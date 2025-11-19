#!/bin/bash

# Zalagaonica Firewall Setup Script for CentOS Stream
# This script opens necessary ports and checks connectivity

set -e

echo "========================================="
echo "Zalagaonica Firewall Setup Script"
echo "========================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Ports to open
PORTS=(3000 3333 5000 5431)

echo "Step 1: Checking Docker containers..."
echo "--------------------------------------"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
echo ""

echo "Step 2: Checking port bindings..."
echo "--------------------------------------"
if command -v netstat &> /dev/null; then
    netstat -tulpn | grep -E ':(3000|3333|5000|5431)' || echo "No ports found with netstat"
elif command -v ss &> /dev/null; then
    ss -tulpn | grep -E ':(3000|3333|5000|5431)' || echo "No ports found with ss"
else
    echo -e "${YELLOW}Warning: Neither netstat nor ss command found${NC}"
fi
echo ""

echo "Step 3: Checking firewalld status..."
echo "--------------------------------------"
if systemctl is-active --quiet firewalld; then
    echo -e "${GREEN}Firewalld is running${NC}"

    echo ""
    echo "Step 3a: Current firewall ports:"
    firewall-cmd --list-ports

    echo ""
    echo "Step 3b: Opening required ports..."
    for port in "${PORTS[@]}"; do
        echo "Opening port ${port}/tcp..."
        firewall-cmd --permanent --add-port=${port}/tcp
    done

    echo ""
    echo "Step 3c: Reloading firewall..."
    firewall-cmd --reload

    echo ""
    echo "Step 3d: Verifying ports are open:"
    firewall-cmd --list-ports

else
    echo -e "${YELLOW}Firewalld is not running, checking iptables...${NC}"

    echo ""
    echo "Step 3e: Current iptables rules:"
    iptables -L INPUT -n --line-numbers | grep -E '3000|3333|5000|5431' || echo "No rules found for our ports"

    echo ""
    echo "Step 3f: Adding iptables rules..."
    for port in "${PORTS[@]}"; do
        # Check if rule already exists
        if ! iptables -C INPUT -p tcp --dport ${port} -j ACCEPT 2>/dev/null; then
            echo "Adding rule for port ${port}/tcp..."
            iptables -A INPUT -p tcp --dport ${port} -j ACCEPT
        else
            echo "Rule for port ${port}/tcp already exists"
        fi
    done

    echo ""
    echo "Step 3g: Saving iptables rules..."
    if command -v iptables-save &> /dev/null; then
        iptables-save > /etc/sysconfig/iptables
        echo "iptables rules saved"
    else
        echo -e "${YELLOW}Warning: Could not save iptables rules${NC}"
    fi
fi
echo ""

echo "Step 4: Checking SELinux status..."
echo "--------------------------------------"
if command -v sestatus &> /dev/null; then
    sestatus

    if sestatus | grep -q "SELinux status:.*enabled"; then
        echo ""
        echo "Step 4a: SELinux is enabled, configuring ports..."

        if command -v semanage &> /dev/null; then
            for port in 3000 3333 5000; do
                echo "Adding SELinux rule for port ${port}..."
                semanage port -a -t http_port_t -p tcp ${port} 2>/dev/null || \
                semanage port -m -t http_port_t -p tcp ${port} 2>/dev/null || \
                echo "Port ${port} already configured or error occurred"
            done
        else
            echo -e "${YELLOW}Warning: semanage command not found. Install policycoreutils-python-utils if needed${NC}"
        fi
    fi
else
    echo "SELinux tools not found, skipping SELinux configuration"
fi
echo ""

echo "Step 5: Testing local connectivity..."
echo "--------------------------------------"

# Test nginx (port 3333)
echo -n "Testing nginx (3333): "
if curl -s -o /dev/null -w "%{http_code}" http://localhost:3333 | grep -q "200\|301\|302\|404"; then
    echo -e "${GREEN}OK${NC}"
else
    echo -e "${RED}FAILED${NC}"
fi

# Test frontend (port 3000)
echo -n "Testing frontend (3000): "
if curl -s -o /dev/null -w "%{http_code}" http://localhost:3000 | grep -q "200\|301\|302\|404"; then
    echo -e "${GREEN}OK${NC}"
else
    echo -e "${RED}FAILED${NC}"
fi

# Test backend health (port 5000)
echo -n "Testing backend (5000): "
if curl -s -o /dev/null -w "%{http_code}" http://localhost:5000/api/health 2>/dev/null | grep -q "200\|404"; then
    echo -e "${GREEN}OK${NC}"
else
    echo -e "${RED}FAILED (might still be starting)${NC}"
fi

# Test database (port 5431)
echo -n "Testing database (5431): "
if nc -z localhost 5431 2>/dev/null; then
    echo -e "${GREEN}OK${NC}"
else
    if timeout 1 bash -c "cat < /dev/null > /dev/tcp/localhost/5431" 2>/dev/null; then
        echo -e "${GREEN}OK${NC}"
    else
        echo -e "${RED}FAILED${NC}"
    fi
fi

echo ""
echo "========================================="
echo "Setup Complete!"
echo "========================================="
echo ""
echo "You should now be able to access:"
echo "  - Frontend: http://193.168.175.225:3000"
echo "  - Nginx: http://193.168.175.225:3333"
echo "  - Backend API: http://193.168.175.225:5000"
echo "  - Database: 193.168.175.225:5431"
echo ""
echo "If you still cannot access from your local PC:"
echo "  1. Check if your hosting provider has additional firewall rules"
echo "  2. Check if there's a network firewall between you and the server"
echo "  3. Try accessing from the server itself first"
echo ""
