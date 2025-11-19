import React, { useState, useEffect, useMemo } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { PlusIcon } from '@heroicons/react/24/outline';
import { Pagination } from '../components/ui/Pagination';
import { CashRegisterTable } from '../components/cash-register/CashRegisterTable';
import { CashRegisterModal } from '../components/cash-register/CashRegisterModal';
import { cashRegisterApi, CashRegisterTransaction } from '../services/cashRegisterApi';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';

export const CashRegisterPage: React.FC = () => {
    // Data state
    const [transactions, setTransactions] = useState<CashRegisterTransaction[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [editingTransaction, setEditingTransaction] = useState<CashRegisterTransaction | null>(null);
    const [transactionToSave, setTransactionToSave] = useState<CashRegisterTransaction | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    // Load transactions from backend
    const loadTransactions = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await cashRegisterApi.getAll();
            setTransactions(data);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to load transactions';
            setError(message);
            console.error('Error loading transactions:', err);
        } finally {
            setLoading(false);
        }
    };

    // Load data on mount
    useEffect(() => {
        loadTransactions();
    }, []);

    const paginatedTransactions = useMemo(() => {
        const firstItemIndex = (currentPage - 1) * itemsPerPage;
        const lastItemIndex = firstItemIndex + itemsPerPage;
        return transactions.slice(firstItemIndex, lastItemIndex);
    }, [transactions, currentPage, itemsPerPage]);
    
    const handleOpenModal = () => {
        setEditingTransaction(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (transaction: CashRegisterTransaction) => {
        setEditingTransaction(transaction);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingTransaction(null);
        setTransactionToSave(null);
    };
    
    const handleSaveTransaction = (transaction: CashRegisterTransaction) => {
        setTransactionToSave(transaction);
        setIsConfirmOpen(true);
    };

    const handleConfirmSave = async () => {
        if (!transactionToSave) return;

        try {
            setLoading(true);

            if (editingTransaction && editingTransaction.id) {
                // Update existing transaction
                await cashRegisterApi.update(editingTransaction.id, transactionToSave);
                setTransactions(prev => prev.map(t => t.id === editingTransaction.id ? transactionToSave : t));
                alert('Transakcija uspješno ažurirana!');
            } else {
                // Create new transaction
                const newTransaction = await cashRegisterApi.create(transactionToSave);
                setTransactions(prev => [...prev, newTransaction]);
                alert('Transakcija uspješno dodana!');
            }

            handleCloseModal();
            setIsConfirmOpen(false);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to save transaction';
            alert('Greška pri spremanju: ' + message);
            console.error('Error saving transaction:', err);
        } finally {
            setLoading(false);
        }
    };

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1); // Reset to first page
    };

    if (loading && transactions.length === 0) {
        return (
            <AppLayout>
                <div className="flex flex-col h-full p-8 items-center justify-center">
                    <div className="text-center">
                        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
                        <p className="mt-4 text-gray-600">Učitavanje transakcija...</p>
                    </div>
                </div>
            </AppLayout>
        );
    }

    if (error && transactions.length === 0) {
        return (
            <AppLayout>
                <div className="flex flex-col h-full p-8">
                    <div className="bg-red-50 border border-red-200 rounded-md p-4">
                        <p className="text-red-800">Greška: {error}</p>
                        <button
                            onClick={loadTransactions}
                            className="mt-2 text-sm text-red-600 hover:text-red-800 underline"
                        >
                            Pokušaj ponovno
                        </button>
                    </div>
                </div>
            </AppLayout>
        );
    }

    return (
        <AppLayout>
            <div className="flex flex-col h-full p-8">
                <Header title="Blagajna" showBackButton={true} />

                {error && (
                    <div className="mt-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
                        <p className="text-yellow-800">Upozorenje: {error}</p>
                    </div>
                )}

                <div className="flex justify-between items-center mt-6">
                    <h2 className="text-2xl font-semibold text-gray-800">Pregled transakcija</h2>
                    <button
                        type="button"
                        onClick={handleOpenModal}
                        disabled={loading}
                        className="inline-flex items-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        <PlusIcon className="h-5 w-5 mr-2" />
                        Novi unos
                    </button>
                </div>

                <div className="mt-8 flow-root flex-grow">
                    <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                        <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                            <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                                {loading && transactions.length > 0 && (
                                    <div className="bg-blue-50 px-4 py-2 text-sm text-blue-700">
                                        Obrada...
                                    </div>
                                )}

                                <CashRegisterTable data={paginatedTransactions} onEdit={handleOpenEditModal} />
                            </div>
                            <Pagination 
                                currentPage={currentPage}
                                totalCount={transactions.length}
                                itemsPerPage={itemsPerPage}
                                onPageChange={setCurrentPage}
                                onItemsPerPageChange={handleItemsPerPageChange}
                            />
                        </div>
                    </div>
                </div>
            </div>
            
            <CashRegisterModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSaveTransaction}
                initialData={editingTransaction}
            />
            
            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={handleConfirmSave}
                title="Potvrda spremanja"
                message="Jeste li sigurni da želite spremiti transakciju?"
            />
        </AppLayout>
    );
};
