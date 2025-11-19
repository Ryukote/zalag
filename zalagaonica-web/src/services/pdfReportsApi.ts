const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

export interface PurchaseReceiptData {
    documentNumber: string;
    documentDate: Date;
    seller: {
        name: string;
        address: string;
        city: string;
        oib: string;
    };
    items: Array<{
        name: string;
        code: string;
        quantity: number;
        unitOfMeasure: string;
        mpc: number;
        purchasePrice: number;
    }>;
    warehouse: string;
    employeeName: string;
}

export interface PledgeAgreementData {
    pledgeNumber: string;
    pledgeDate: Date;
    client: {
        name: string;
        address?: string;
        city?: string;
        oib?: string;
    };
    item: {
        name: string;
        description: string;
        estimatedValue: number;
    };
    loanAmount: number;
    returnAmount: number;
    period: number;
    redeemDeadline: Date;
}

export interface InboundCalculationData {
    documentNumber: string;
    documentDate: Date;
    incomingDocumentNumber: string;
    incomingDocumentDate: Date;
    seller: {
        name: string;
        oib: string;
    };
    warehouse: string;
    items: Array<{
        name: string;
        description: string;
        quantity: number;
        unitOfMeasure: string;
        invoicePrice: number;
        discountPercent: number;
        discountAmount: number;
        purchasePrice: number;
        marginPercent: number;
        marginAmount: number;
        taxPercent: number;
        taxAmount: number;
        retailPrice: number;
    }>;
    totalInvoicePrice: number;
    totalPurchasePrice: number;
    totalMargin: number;
    totalTax: number;
    totalRetailPrice: number;
    vatOnAddedValue: {
        base: number;
        amount: number;
    };
}

export interface AppraisalRequestData {
    documentNumber: string;
    documentDate: Date;
    client: {
        name: string;
        address?: string;
        city?: string;
        oib?: string;
    };
    item: {
        name: string;
        description: string;
        estimatedValue: number;
    };
    purpose: string;
}

export interface ReservationReceiptData {
    documentNumber: string;
    documentDate: Date;
    client: {
        name: string;
        address?: string;
        city?: string;
        oib?: string;
    };
    items: Array<{
        name: string;
        description: string;
        price: number;
        quantity: number;
    }>;
    reservationDeposit: number;
    reservationUntil: Date;
    employeeName: string;
}

export interface WarehouseTransferData {
    documentNumber: string;
    documentDate: Date;
    fromWarehouse: string;
    toWarehouse: string;
    items: Array<{
        name: string;
        code: string;
        quantity: number;
        unitOfMeasure: string;
        unitPrice: number;
        totalPrice: number;
    }>;
    employeeName: string;
    notes?: string;
}

const downloadPdf = (blob: Blob, filename: string) => {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
};

export const generatePurchaseReceipt = async (data: PurchaseReceiptData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/purchase-receipt`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `otkupni-blok-${data.documentNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating purchase receipt:', error);
        throw error;
    }
};

export const generatePledgeAgreement = async (data: PledgeAgreementData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/pledge-agreement`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `ugovor-zalog-${data.pledgeNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating pledge agreement:', error);
        throw error;
    }
};

export const generateInboundCalculation = async (data: InboundCalculationData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/inbound-calculation`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `ulazna-kalkulacija-${data.documentNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating inbound calculation:', error);
        throw error;
    }
};

export const generateAppraisalRequest = async (data: AppraisalRequestData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/appraisal-request`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `zahtjev-procjena-${data.documentNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating appraisal request:', error);
        throw error;
    }
};

export const generateReservationReceipt = async (data: ReservationReceiptData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/reservation-receipt`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `potvrda-rezervacije-${data.documentNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating reservation receipt:', error);
        throw error;
    }
};

export const generateWarehouseTransfer = async (data: WarehouseTransferData): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/PdfReports/warehouse-transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to generate PDF');
        }

        const blob = await response.blob();
        const filename = `medjuskladisnica-${data.documentNumber}.pdf`;
        downloadPdf(blob, filename);
    } catch (error) {
        console.error('Error generating warehouse transfer:', error);
        throw error;
    }
};
