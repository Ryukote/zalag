import React, { useState, useEffect } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon, ArrowDownTrayIcon } from '@heroicons/react/24/outline';
import { PriceListModal } from '../components/price-list/PriceListModal';
import { PriceListTable } from '../components/price-list/PriceListTable';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import { PriceListItem } from '../types/priceListItem';
import { Article, articleApi } from '../services/articleApi';

// Helper function to convert Article to PriceListItem for display
const articleToPriceListItem = (article: Article): PriceListItem => {
    const retailPriceWithTax = article.retailPrice * (1 + article.taxRate / 100);
    return {
        id: article.id,
        name: article.name,
        stock: article.stock,
        unitOfMeasure: article.unitOfMeasureCode,
        retailPrice: article.retailPrice,
        retailPriceWithTax,
        taxRate: article.taxRate,
        supplierName: article.supplierName || '',
        group: article.group,
        taxTariffNumber: undefined, // Not available in Article
    };
};

export const PriceListPage: React.FC = () => {
    const [priceListItems, setPriceListItems] = useState<PriceListItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingItem, setEditingItem] = useState<PriceListItem | null>(null);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [itemToSave, setItemToSave] = useState<PriceListItem | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    useEffect(() => {
        loadArticles();
    }, []);

    const loadArticles = async () => {
        try {
            setLoading(true);
            const articles = await articleApi.getAll();
            const priceListData = articles.map(articleToPriceListItem);
            setPriceListItems(priceListData);
        } catch (error) {
            console.error('Failed to load price list:', error);
            alert('Failed to load price list');
        } finally {
            setLoading(false);
        }
    };

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    const handleOpenAddModal = () => {
        setEditingItem(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (item: PriceListItem) => {
        setEditingItem(item);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingItem(null);
    };

    const handleSaveItem = (itemData: PriceListItem) => {
        setItemToSave(itemData);
        setIsModalOpen(false);
        setIsConfirmOpen(true);
    };

    const handleConfirmSave = async () => {
        if (!itemToSave) return;

        try {
            // Note: This is a simplified version. In reality, you'd need to convert
            // PriceListItem back to Article and call the API
            // For now, just reload the data
            alert('Article update via price list is not yet fully implemented. Please use the Articles page.');
            setIsConfirmOpen(false);
            setItemToSave(null);
            setEditingItem(null);
        } catch (error) {
            console.error('Failed to save price list item:', error);
            alert('Failed to save price list item');
        }
    };

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = priceListItems.slice(indexOfFirstItem, indexOfLastItem);

    return (
        <AppLayout>
            <div className="px-6 py-8 bg-gray-50 min-h-screen">
                <Header title="Cjenik - Lager Lista" showBackButton={true} />

                <div className="mt-6 md:flex md:items-center md:justify-between">
                    <div className="flex-1 min-w-0">
                        {/* Ovdje će ići filteri specifični za cjenik */}
                    </div>
                    <div className="mt-4 flex-shrink-0 flex md:mt-0 md:ml-4">
                        <button
                          type="button"
                          className="flex items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                        >
                          <ArrowDownTrayIcon className="h-5 w-5 mr-2 text-gray-500" />
                          Izvoz
                        </button>
                        <button
                          type="button"
                          onClick={handleOpenAddModal}
                          className="ml-3 flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
                        >
                          <PlusIcon className="h-5 w-5 mr-2" />
                          Nova Stavka
                        </button>
                    </div>
                </div>

                <div className="mt-8 flow-root">
                    <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                        <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                            {loading ? (
                                <div className="text-center py-4">Loading...</div>
                            ) : (
                                <>
                                    <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                                        <PriceListTable data={currentItems} onEdit={handleOpenEditModal} />
                                    </div>
                                    <Pagination
                                        currentPage={currentPage}
                                        totalCount={priceListItems.length}
                                        itemsPerPage={itemsPerPage}
                                        onPageChange={page => setCurrentPage(page)}
                                        onItemsPerPageChange={handleItemsPerPageChange}
                                    />
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </div>

            <PriceListModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSaveItem}
                initialData={editingItem}
            />
            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={handleConfirmSave}
                title="Potvrda izmjena"
                message="Jeste li sigurni da želite spremiti promjene?"
            />
        </AppLayout>
    );
};
