#!/bin/bash

# Comprehensive Network Fix and Diagnostic Script
# Run this on your CentOS server

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;36m'
NC='\033[0m'

echo -e "${BLUE}=========================================${NC}"
echo -e "${BLUE}Zalagaonica Network Diagnostic & Fix${NC}"
echo -e "${BLUE}=========================================${NC}"
echo ""

# Step 1: Verify docker-compose.yml has correct bindings
echo -e "${YELLOW}Step 1: Checking docker-compose.yml port bindings...${NC}"
if grep -q "0.0.0.0:3000:80" docker-compose.yml && \
   grep -q "0.0.0.0:3333" docker-compose.yml && \
   grep -q "0.0.0.0:5000:80" docker-compose.yml && \
   grep -q "0.0.0.0:5431:5432" docker-compose.yml; then
    echo -e "${GREEN}✓ docker-compose.yml has correct 0.0.0.0 bindings${NC}"
else
    echo -e "${RED}✗ docker-compose.yml is missing 0.0.0.0 bindings${NC}"
    echo -e "${RED}Please upload the updated docker-compose.yml file!${NC}"
    exit 1
fi
echo ""

# Step 2: Restart containers
echo -e "${YELLOW}Step 2: Restarting containers with new configuration...${NC}"
docker compose down
sleep 3
docker compose up -d
echo "Waiting 30 seconds for containers to start..."
sleep 30
echo ""

# Step 3: Check container status
echo -e "${YELLOW}Step 3: Checking container status...${NC}"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
echo ""

# Step 4: Check port bindings
echo -e "${YELLOW}Step 4: Checking if ports are bound to all interfaces (0.0.0.0 or :::)...${NC}"
echo "Port bindings:"
if command -v netstat &> /dev/null; then
    netstat -tlnp | grep -E ':(3000|3333|5000|5431)' || echo "No ports found"
elif command -v ss &> /dev/null; then
    ss -tlnp | grep -E ':(3000|3333|5000|5431)' || echo "No ports found"
fi
echo ""

# Check if ports are bound to localhost only (BAD)
if netstat -tln 2>/dev/null | grep -E '127\.0\.0\.1:(3000|3333|5000|5431)' || \
   ss -tln 2>/dev/null | grep -E '127\.0\.0\.1:(3000|3333|5000|5431)'; then
    echo -e "${RED}✗ PROBLEM: Ports are bound to localhost (127.0.0.1) only!${NC}"
    echo -e "${RED}  Docker is not binding to all interfaces correctly.${NC}"
    echo ""
fi

# Step 5: Configure firewall
echo -e "${YELLOW}Step 5: Configuring firewall...${NC}"
if systemctl is-active --quiet firewalld; then
    echo "Firewalld is active. Opening ports..."
    firewall-cmd --permanent --add-port=3000/tcp
    firewall-cmd --permanent --add-port=3333/tcp
    firewall-cmd --permanent --add-port=5000/tcp
    firewall-cmd --permanent --add-port=5431/tcp
    firewall-cmd --reload
    echo -e "${GREEN}✓ Firewall ports opened${NC}"
    echo "Open ports:"
    firewall-cmd --list-ports
else
    echo "Firewalld not active. Checking iptables..."
    # Add iptables rules
    iptables -C INPUT -p tcp --dport 3000 -j ACCEPT 2>/dev/null || iptables -I INPUT -p tcp --dport 3000 -j ACCEPT
    iptables -C INPUT -p tcp --dport 3333 -j ACCEPT 2>/dev/null || iptables -I INPUT -p tcp --dport 3333 -j ACCEPT
    iptables -C INPUT -p tcp --dport 5000 -j ACCEPT 2>/dev/null || iptables -I INPUT -p tcp --dport 5000 -j ACCEPT
    iptables -C INPUT -p tcp --dport 5431 -j ACCEPT 2>/dev/null || iptables -I INPUT -p tcp --dport 5431 -j ACCEPT
    echo -e "${GREEN}✓ iptables rules added${NC}"
fi
echo ""

# Step 6: Check SELinux
echo -e "${YELLOW}Step 6: Checking SELinux...${NC}"
if command -v getenforce &> /dev/null; then
    SELINUX_STATUS=$(getenforce)
    echo "SELinux status: $SELINUX_STATUS"

    if [ "$SELINUX_STATUS" = "Enforcing" ]; then
        if command -v semanage &> /dev/null; then
            echo "Configuring SELinux ports..."
            semanage port -a -t http_port_t -p tcp 3000 2>/dev/null || semanage port -m -t http_port_t -p tcp 3000
            semanage port -a -t http_port_t -p tcp 3333 2>/dev/null || semanage port -m -t http_port_t -p tcp 3333
            semanage port -a -t http_port_t -p tcp 5000 2>/dev/null || semanage port -m -t http_port_t -p tcp 5000
            echo -e "${GREEN}✓ SELinux configured${NC}"
        else
            echo -e "${YELLOW}⚠ semanage not found. You may need to install: yum install policycoreutils-python-utils${NC}"
        fi
    fi
else
    echo "SELinux not found or not installed"
fi
echo ""

# Step 7: Test local connectivity
echo -e "${YELLOW}Step 7: Testing local connectivity...${NC}"

test_port() {
    local port=$1
    local path=$2
    local name=$3

    echo -n "Testing $name (port $port): "
    if timeout 5 curl -s -o /dev/null -w "%{http_code}" "http://localhost:$port$path" | grep -qE "200|301|302|404"; then
        echo -e "${GREEN}OK${NC}"
        return 0
    else
        echo -e "${RED}FAILED${NC}"
        return 1
    fi
}

test_port 3333 "/" "Nginx"
test_port 3000 "/" "Frontend"
test_port 5000 "/api/health" "Backend API"

echo -n "Testing Database (port 5431): "
if timeout 2 bash -c "cat < /dev/null > /dev/tcp/localhost/5431" 2>/dev/null; then
    echo -e "${GREEN}OK${NC}"
else
    echo -e "${RED}FAILED${NC}"
fi
echo ""

# Step 8: Test external connectivity
echo -e "${YELLOW}Step 8: Testing external connectivity...${NC}"
SERVER_IP=$(hostname -I | awk '{print $1}')
echo "Server IP detected: $SERVER_IP"
echo ""

echo -n "Testing from server IP ($SERVER_IP:3333): "
if timeout 5 curl -s -o /dev/null -w "%{http_code}" "http://$SERVER_IP:3333/" | grep -qE "200|301|302|404"; then
    echo -e "${GREEN}OK - Accessible from external IP!${NC}"
else
    echo -e "${RED}FAILED - Not accessible from external IP${NC}"
    echo -e "${RED}This could be a cloud provider firewall/security group issue${NC}"
fi
echo ""

# Step 9: Final diagnostics
echo -e "${YELLOW}Step 9: Final diagnostic information...${NC}"
echo ""
echo "Docker network inspect:"
docker network inspect zalagaonica-network | grep -A 3 "Containers"
echo ""

echo -e "${BLUE}=========================================${NC}"
echo -e "${BLUE}Summary${NC}"
echo -e "${BLUE}=========================================${NC}"
echo ""
echo "Application URLs:"
echo "  - Frontend:      http://$SERVER_IP:3000"
echo "  - Nginx (main):  http://$SERVER_IP:3333"
echo "  - Backend API:   http://$SERVER_IP:5000/api/health"
echo "  - Database:      $SERVER_IP:5431"
echo ""
echo -e "${YELLOW}If you still cannot access from your PC:${NC}"
echo ""
echo "1. CHECK CLOUD PROVIDER FIREWALL:"
echo "   - If this is a VPS/Cloud server (AWS, Azure, GCP, DigitalOcean, etc.)"
echo "   - You MUST open ports 3000, 3333, 5000, 5431 in:"
echo "     * AWS: Security Groups"
echo "     * Azure: Network Security Groups (NSG)"
echo "     * GCP: Firewall Rules"
echo "     * DigitalOcean: Cloud Firewalls"
echo "     * Other: Check your provider's firewall/security settings"
echo ""
echo "2. CHECK YOUR LOCAL FIREWALL:"
echo "   - Windows Firewall may be blocking outbound connections"
echo "   - Antivirus software may be blocking connections"
echo ""
echo "3. TRY FROM ANOTHER LOCATION:"
echo "   - Try from your phone's mobile data (not WiFi)"
echo "   - This will confirm if it's your local network blocking it"
echo ""
echo "4. CHECK PORT SCAN FROM EXTERNAL:"
echo "   - Use: https://www.yougetsignal.com/tools/open-ports/"
echo "   - Enter your server IP and check if port 3333 is open"
echo ""
