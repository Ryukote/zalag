import React, { useState, useEffect, useMemo } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon, ArrowDownTrayIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { outputDocumentApi } from '../services/outputDocumentApi';

// --- INTERFACES ---

export interface OutputDocument {
  id: string;
  clientName: string;
  documentNumber: string;
  documentDate: string;
  totalValue: number;
  status: 'otvoren' | 'proknjižen';
  documentType: string;
  year: number;
  operator: string;
  note?: string;
  isPosted: boolean;
  totalWithTax: number;
  pretaxAmount: number;
}

// Reusable utility function for formatting currency
const formatCurrency = (amount: number) => {
  return new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);
};

// --- TABLE COMPONENT ---
interface OutputDocumentListTableProps {
    documents: OutputDocument[];
    onEdit: (document: OutputDocument) => void;
}

const OutputDocumentListTable: React.FC<OutputDocumentListTableProps> = ({ documents, onEdit }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">Komitent</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Br. Dokumenta</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Datum</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">Vrijednost</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Status</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Tip</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Akcije</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {documents.map((doc) => (
                    <tr key={doc.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{doc.clientName}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{doc.documentNumber}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{doc.documentDate}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(doc.totalValue)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm border-l border-gray-200">
                            <span className={`inline-flex items-center rounded-md px-2 py-1 text-xs font-medium ${
                                doc.status === 'proknjižen' 
                                ? 'bg-green-50 text-green-700 ring-1 ring-inset ring-green-600/20' 
                                : 'bg-yellow-50 text-yellow-800 ring-1 ring-inset ring-yellow-600/20'
                            }`}>
                                {doc.status === 'proknjižen' ? 'Proknjižen' : 'Otvoren'}
                            </span>
                        </td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{doc.documentType}</td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-center text-sm font-medium border-l border-gray-200">
                            <button onClick={() => onEdit(doc)} className="text-indigo-600 hover:text-indigo-900">
                                <ArrowDownTrayIcon className="h-5 w-5" />
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

// --- MODAL COMPONENT (with form) ---
interface OutputDocumentModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (document: OutputDocument) => void;
  initialData: OutputDocument | null;
}

const defaultDocument: OutputDocument = {
  id: '',
  clientName: '',
  documentNumber: '',
  documentDate: new Date().toISOString().split('T')[0],
  totalValue: 0,
  status: 'otvoren',
  documentType: 'RAČUN',
  year: new Date().getFullYear(),
  operator: '',
  note: '',
  isPosted: false,
  totalWithTax: 0,
  pretaxAmount: 0
};

const OutputDocumentModal: React.FC<OutputDocumentModalProps> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState<OutputDocument>(defaultDocument);

  useEffect(() => {
    setFormData(initialData || defaultDocument);
  }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    const isNumber = type === 'number' || type === 'range';
    setFormData(prev => ({
      ...prev,
      [name]: isNumber ? parseFloat(value) || 0 : value
    }));
  };

  const handleSave = () => {
    onSave(formData);
  };

  return (
    <div className={`fixed inset-0 z-50 overflow-y-auto ${isOpen ? '' : 'hidden'}`}>
      <div className="flex items-center justify-center min-h-screen px-4 py-8 text-center">
        <div className="fixed inset-0 transition-opacity bg-gray-500 bg-opacity-75" onClick={onClose}></div>
        <div className="relative inline-block w-full max-w-2xl p-6 my-8 overflow-hidden text-left align-middle transition-all transform bg-white rounded-lg shadow-xl">
          <h3 className="text-lg font-medium leading-6 text-gray-900">
            {initialData ? 'Uredi Izlazni Dokument' : 'Novi Izlazni Dokument'}
          </h3>
          <form className="mt-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-4">
              <div>
                <label htmlFor="clientName" className="block text-sm font-medium leading-6 text-gray-900">Naziv Komitenta</label>
                <input type="text" name="clientName" id="clientName" value={formData.clientName} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300" />
              </div>
              <div>
                <label htmlFor="documentNumber" className="block text-sm font-medium leading-6 text-gray-900">Broj Dokumenta</label>
                <input type="text" name="documentNumber" id="documentNumber" value={formData.documentNumber} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300" />
              </div>
              <div>
                <label htmlFor="documentDate" className="block text-sm font-medium leading-6 text-gray-900">Datum Dokumenta</label>
                <input type="date" name="documentDate" id="documentDate" value={formData.documentDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300" />
              </div>
              <div>
                <label htmlFor="documentType" className="block text-sm font-medium leading-6 text-gray-900">Tip Dokumenta</label>
                <select name="documentType" id="documentType" value={formData.documentType} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300">
                    <option>RAČUN</option>
                    <option>OTPREMNICA</option>
                </select>
              </div>
              <div>
                <label htmlFor="totalValue" className="block text-sm font-medium leading-6 text-gray-900">Ukupna Vrijednost (bez poreza)</label>
                <input type="number" name="totalValue" id="totalValue" value={formData.totalValue} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300" />
              </div>
              <div>
                <label htmlFor="note" className="block text-sm font-medium leading-6 text-gray-900">Napomena</label>
                <textarea name="note" id="note" value={formData.note} onChange={handleChange} rows={2} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300" />
              </div>
            </div>

            <div className="mt-8 flex justify-end space-x-3">
              <button type="button" onClick={onClose} className="rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">
                Odustani
              </button>
              <button type="button" onClick={handleSave} className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">
                Spremi
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

// --- MAIN PAGE COMPONENT ---
export const OutputDocumentsPage = () => {
    const [documents, setDocuments] = useState<OutputDocument[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTerm, setSearchTerm] = useState('');
    const [statusFilter, setStatusFilter] = useState('svi');
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingDocument, setEditingDocument] = useState<OutputDocument | null>(null);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);

    useEffect(() => {
        loadDocuments();
    }, []);

    const loadDocuments = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await outputDocumentApi.getAll();
            setDocuments(data);
        } catch (err: any) {
            console.error('Error loading output documents:', err);
            setError(err.message || 'Greška pri učitavanju izlaznih dokumenata');
        } finally {
            setLoading(false);
        }
    };

    const handleOpenModal = () => {
        setEditingDocument(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (document: OutputDocument) => {
        setEditingDocument(document);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingDocument(null);
    };

    const handleSave = (newDocument: OutputDocument) => {
        setEditingDocument(newDocument);
        setIsConfirmOpen(true);
    };

    const handleConfirmSave = async () => {
        if (!editingDocument) return;

        try {
            if (editingDocument.id) {
                // Update existing document
                await outputDocumentApi.update(editingDocument.id, {
                    id: editingDocument.id,
                    clientName: editingDocument.clientName,
                    documentNumber: editingDocument.documentNumber,
                    documentDate: editingDocument.documentDate,
                    totalValue: editingDocument.totalValue,
                    status: editingDocument.status,
                    documentType: editingDocument.documentType,
                    operator: editingDocument.operator,
                    note: editingDocument.note,
                    isPosted: editingDocument.isPosted,
                    totalWithTax: editingDocument.totalWithTax,
                    pretaxAmount: editingDocument.pretaxAmount
                });
            } else {
                // Create new document
                await outputDocumentApi.create({
                    clientName: editingDocument.clientName,
                    documentNumber: editingDocument.documentNumber,
                    documentDate: editingDocument.documentDate,
                    totalValue: editingDocument.totalValue,
                    status: editingDocument.status,
                    documentType: editingDocument.documentType,
                    operator: editingDocument.operator,
                    note: editingDocument.note,
                    isPosted: editingDocument.isPosted,
                    totalWithTax: editingDocument.totalWithTax,
                    pretaxAmount: editingDocument.pretaxAmount
                });
            }
            setIsConfirmOpen(false);
            handleCloseModal();
            await loadDocuments();
        } catch (err: any) {
            alert('Greška pri spremanju: ' + (err.message || 'Nepoznata greška'));
        }
    };

    const filteredDocuments = useMemo(() => {
        return documents.filter(doc => {
            const matchesSearch = searchTerm === '' ||
                                  doc.clientName.toLowerCase().includes(searchTerm.toLowerCase()) ||
                                  doc.documentNumber.toLowerCase().includes(searchTerm.toLowerCase());
            
            const matchesStatus = statusFilter === 'svi' ||
                                  doc.status === statusFilter;
            
            return matchesSearch && matchesStatus;
        });
    }, [documents, searchTerm, statusFilter]);

    const currentDocuments = useMemo(() => {
        const firstItemIndex = (currentPage - 1) * itemsPerPage;
        const lastItemIndex = firstItemIndex + itemsPerPage;
        return filteredDocuments.slice(firstItemIndex, lastItemIndex);
    }, [filteredDocuments, currentPage, itemsPerPage]);

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    if (loading) {
        return (
            <AppLayout>
                <LoadingSpinner fullScreen message="Učitavanje izlaznih dokumenata..." />
            </AppLayout>
        );
    }

    return (
        <AppLayout>
            <div className="flex flex-col flex-1 p-8">
                <Header title="Izlazni dokumenti" showBackButton />

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

                {/* Sekcija s gumbima i filterima */}
                <div className="mt-6 flex flex-col md:flex-row md:items-center md:justify-between space-y-4 md:space-y-0">
                    <div className="flex items-center space-x-4">
                        <div className="relative rounded-md shadow-sm">
                            <input
                                type="text"
                                name="search"
                                id="search"
                                className="block w-full rounded-md border-0 py-1.5 pr-10 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                                placeholder="Pretraži..."
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                        <div className="relative">
                            <label htmlFor="status" className="sr-only">Status</label>
                            <select
                                id="status"
                                name="status"
                                className="block w-full rounded-md border-0 py-1.5 pl-3 pr-10 text-gray-900 ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-indigo-600 sm:text-sm"
                                value={statusFilter}
                                onChange={(e) => setStatusFilter(e.target.value)}
                            >
                                <option value="svi">Svi</option>
                                <option value="otvoren">Otvoren</option>
                                <option value="proknjižen">Proknjižen</option>
                            </select>
                        </div>
                    </div>
                    <div className="flex items-center space-x-3">
                        <button
                          type="button"
                          className="flex items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                        >
                            <ArrowDownTrayIcon className="h-5 w-5 mr-2 text-gray-500" />
                            Ispis Liste
                        </button>
                        <button
                          type="button"
                          onClick={handleOpenModal}
                          className="flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
                        >
                          <PlusIcon className="h-5 w-5 mr-2" />
                          Novi dokument
                        </button>
                    </div>
                </div>

                <div className="mt-8 flow-root">
                    <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                        <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                            <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                                <OutputDocumentListTable
                                    documents={currentDocuments}
                                    onEdit={handleOpenEditModal}
                                />
                            </div>
                            <Pagination 
                                currentPage={currentPage}
                                totalCount={filteredDocuments.length}
                                itemsPerPage={itemsPerPage}
                                onPageChange={setCurrentPage}
                                onItemsPerPageChange={handleItemsPerPageChange}
                            />
                        </div>
                    </div>
                </div>
            </div>
            
            <OutputDocumentModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSave}
                initialData={editingDocument}
            />
            
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
