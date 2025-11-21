import React, { useState, useEffect } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import { IncomingDocumentModal } from '../components/incoming-documents/IncomingDocumentModal';
import { IncomingDocumentListTable } from '../components/incoming-documents/IncomingDocumentListTable';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { incomingDocumentApi, IncomingDocument } from '../services/incomingDocumentApi';
import * as PdfReportsApi from '../services/pdfReportsApi';

export const IncomingDocumentsPage: React.FC = () => {
    const [documents, setDocuments] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedDocument, setSelectedDocument] = useState<any | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingDocument, setEditingDocument] = useState<any | null>(null);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [documentToSave, setDocumentToSave] = useState<any | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    useEffect(() => {
        loadDocuments();
    }, []);

    const loadDocuments = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await incomingDocumentApi.getAll();
            // Map API data to match component expectations
            const mapped = data.map(d => ({
                id: d.id,
                supplierName: d.supplierName,
                documentNumber: d.documentNumber,
                documentDate: d.documentDate,
                bookingDate: d.documentDate,
                invoiceValue: d.totalAmount,
                purchaseValue: d.totalAmount,
                totalPaid: d.totalAmount,
                retailValue: d.totalAmount,
                margin: 0,
                tax: 0,
                status: 'proknjižen',
                warehouseName: 'Glavno skladište',
                documentType: 'PRIMKA',
                year: new Date(d.documentDate).getFullYear(),
                operator: 'Admin',
                dueDate: d.documentDate,
                isPosted: true,
                discount: 0,
                cost: 0,
                wholesaleValue: 0,
                vatAmount: 0,
                returnFee: 0,
                totalWithReturnFee: d.totalAmount,
                pretaxAmount: 0,
                note: d.description || ''
            }));
            setDocuments(mapped);
        } catch (err: any) {
            console.error('Error loading documents:', err);
            setError(err.message || 'Greška pri učitavanju dokumenata');
        } finally {
            setLoading(false);
        }
    };

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    const handleOpenAddModal = () => {
        setEditingDocument(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (doc: any) => {
        setEditingDocument(doc);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
    };

    const handleSave = async (docData: any) => {
        try {
            if (editingDocument) {
                await incomingDocumentApi.update(docData.id, {
                    documentNumber: docData.documentNumber,
                    supplierName: docData.supplierName,
                    documentDate: docData.documentDate,
                    totalAmount: docData.invoiceValue,
                    description: docData.note
                });
            } else {
                await incomingDocumentApi.create({
                    documentNumber: docData.documentNumber,
                    supplierName: docData.supplierName,
                    documentDate: docData.documentDate,
                    totalAmount: docData.invoiceValue,
                    description: docData.note
                });
            }
            setIsModalOpen(false);
            await loadDocuments();
        } catch (err: any) {
            alert('Greška: ' + (err.message || 'Nepoznata greška'));
        }
    };

    const handleGenerateReport = async (doc: any) => {
        try {
            const data: PdfReportsApi.InboundCalculationData = {
                documentNumber: doc.documentNumber,
                documentDate: new Date(doc.documentDate),
                incomingDocumentNumber: doc.documentNumber,
                incomingDocumentDate: new Date(doc.documentDate),
                seller: {
                    name: doc.supplierName,
                    oib: '12345678901'
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

    if (loading) {
        return (
            <AppLayout>
                <LoadingSpinner fullScreen message="Učitavanje ulaznih dokumenata..." />
            </AppLayout>
        );
    }

    const currentDocuments = documents.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    return (
        <AppLayout>
            <div className="px-6 py-8 bg-gray-50 min-h-screen flex flex-col">
                <Header title="Ulazni dokumenti" showBackButton={true} />

                {error && (
                    <div className="mt-4 bg-red-50 border-l-4 border-red-400 p-4">
                        <div className="flex">
                            <ExclamationCircleIcon className="h-5 w-5 text-red-400 mr-2" />
                            <div>
                                <p className="text-sm text-red-700">{error}</p>
                                <button onClick={loadDocuments} className="text-sm text-red-600 underline mt-1">
                                    Pokušaj ponovno
                                </button>
                            </div>
                        </div>
                    </div>
                )}

                <div className="mt-6 p-4 bg-white rounded-lg shadow-sm border border-gray-200 flex items-center justify-between">
                    <div className="flex items-center gap-4">
                        <span className="text-sm text-gray-600">Ukupno dokumenata: {documents.length}</span>
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

            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={() => {}}
                title="Potvrda spremanja"
                message="Jeste li sigurni da želite spremiti dokument?"
            />
        </AppLayout>
    );
};
