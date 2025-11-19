import React, { useState, useEffect, useMemo } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { IncomingDocument } from '../types/incomingDocument';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon, PencilIcon } from '@heroicons/react/24/outline';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import { IncomingDocumentModal } from '../components/incoming-documents/IncomingDocumentModal';
import { IncomingDocumentListTable } from '../components/incoming-documents/IncomingDocumentListTable';
import * as PdfReportsApi from '../services/pdfReportsApi';

const MOCK_DOCUMENTS: IncomingDocument[] = [
    // ... vaši mock podaci
    { id: 30, supplierName: 'PURKIĆ JANES D.O.O.', bookingDate: '2025-06-03', documentNumber: '08 REZ 234/25', documentDate: '2025-06-03', purchaseValue: -30, margin: -30, tax: 0, status: 'proknjižen', warehouseName: 'Zalagaonica (ZG3)', documentType: 'PRIMKA', year: 2025, operator: 'TAMARA', dueDate: '2025-06-03', isPosted: true, invoiceValue: -30, discount: 0, cost: 0, wholesaleValue: 0, vatAmount: 0, retailValue: -30, returnFee: 0, totalWithReturnFee: -30, pretaxAmount: 0, totalPaid: -30, note: 'Testna napomena' },
    { id: 29, supplierName: 'MARLON PAS CU', bookingDate: '2025-05-23', documentNumber: '08 REZ 12/25', documentDate: '2025-05-23', purchaseValue: -800, margin: -800, tax: 0, status: 'proknjižen', warehouseName: 'Zalagaonica (ZG3)', documentType: 'PRIMKA', year: 2025, operator: 'TAMARA', dueDate: '2025-05-23', isPosted: true, invoiceValue: -800, discount: 0, cost: 0, wholesaleValue: 0, vatAmount: 0, retailValue: -800, returnFee: 0, totalWithReturnFee: -800, pretaxAmount: 0, totalPaid: -800 },
];

export const IncomingDocumentsPage: React.FC = () => {
    const [documents, setDocuments] = useState<IncomingDocument[]>([]);
    const [selectedDocument, setSelectedDocument] = useState<IncomingDocument | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingDocument, setEditingDocument] = useState<IncomingDocument | null>(null);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [documentToSave, setDocumentToSave] = useState<IncomingDocument | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [isEditingForm, setIsEditingForm] = useState(false);

    useEffect(() => {
        setDocuments(MOCK_DOCUMENTS);
    }, []);

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    const handleOpenAddModal = () => {
        setEditingDocument(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (doc: IncomingDocument) => {
        setEditingDocument(doc);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
    };

    const handleSave = (docData: IncomingDocument) => {
        setDocumentToSave(docData);
        setIsModalOpen(false);
        setIsConfirmOpen(true);
    };

    const handleConfirmSave = () => {
        if (!documentToSave) return;
        if (editingDocument) {
            setDocuments(prev => prev.map(d => d.id === documentToSave.id ? documentToSave : d));
        } else {
            setDocuments(prev => [{...documentToSave, id: Date.now()}, ...prev]);
        }
        setIsConfirmOpen(false);
        setDocumentToSave(null);
        setEditingDocument(null);
    };

    const handleGenerateReport = async (doc: IncomingDocument) => {
        try {
            // Generate Inbound Calculation report
            // This is a demo - you would map real item data from your document details
            const data: PdfReportsApi.InboundCalculationData = {
                documentNumber: doc.documentNumber,
                documentDate: new Date(doc.documentDate),
                incomingDocumentNumber: doc.documentNumber,
                incomingDocumentDate: new Date(doc.documentDate),
                seller: {
                    name: doc.supplierName,
                    oib: '12345678901' // This would come from supplier data
                },
                warehouse: doc.warehouseName,
                items: [{
                    name: 'Sample Item',
                    description: `Document ${doc.documentNumber}`,
                    quantity: 1,
                    unitOfMeasure: 'KOM',
                    invoicePrice: Math.abs(doc.invoiceValue),
                    discountPercent: doc.discount,
                    discountAmount: Math.abs(doc.invoiceValue * (doc.discount / 100)),
                    purchasePrice: Math.abs(doc.purchaseValue),
                    marginPercent: (doc.margin / Math.abs(doc.purchaseValue)) * 100,
                    marginAmount: doc.margin,
                    taxPercent: doc.tax,
                    taxAmount: doc.vatAmount,
                    retailPrice: Math.abs(doc.retailValue)
                }],
                totalInvoicePrice: Math.abs(doc.invoiceValue),
                totalPurchasePrice: Math.abs(doc.purchaseValue),
                totalMargin: doc.margin,
                totalTax: doc.tax,
                totalRetailPrice: Math.abs(doc.retailValue),
                vatOnAddedValue: {
                    base: Math.abs(doc.purchaseValue),
                    amount: doc.vatAmount
                }
            };

            await PdfReportsApi.generateInboundCalculation(data);
        } catch (error) {
            console.error('Error generating inbound calculation:', error);
            alert('Došlo je do greške pri generiranju PDF-a');
        }
    };

    const currentDocuments = documents.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    return (
        <AppLayout>
            <div className="px-6 py-8 bg-gray-50 min-h-screen flex flex-col">
                <Header title="Ulazni dokumenti" showBackButton={true} />
                
                <div className="mt-6 p-4 bg-white rounded-lg shadow-sm border border-gray-200 flex items-center justify-between">
                    <div className="flex items-center gap-4">
                        {/* Filteri */}
                    </div>
                    <button
                        type="button"
                        onClick={handleOpenAddModal}
                        className="flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
                    >
                        <PlusIcon className="h-5 w-5 mr-2" />
                        Novi dokument
                    </button>
                </div>

                <div className="mt-8 flex-grow flex flex-col">
                    <IncomingDocumentListTable
                        documents={currentDocuments}
                        onEdit={handleOpenEditModal}
                        onGenerateReport={handleGenerateReport}
                    />
                    
                    {/* OVDJE SU SADA ISPRAVNO DODANI SVI POTREBNI PROPS */}
                    <Pagination 
                        currentPage={currentPage}
                        totalCount={documents.length}
                        itemsPerPage={itemsPerPage}
                        onPageChange={setCurrentPage}
                        onItemsPerPageChange={handleItemsPerPageChange}
                    />
                </div>
            </div>
            
            <IncomingDocumentModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSave}
                initialData={editingDocument}
            />
            
            {/* I OVDJE SU SADA ISPRAVNO DODANI SVI POTREBNI PROPS */}
            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={handleConfirmSave}
                title="Potvrda spremanja"
                message="Jeste li sigurni da želite spremiti dokument?"
            />
        </AppLayout>
    );
};