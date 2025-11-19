import React, { useState, useEffect } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon, ArrowDownTrayIcon } from '@heroicons/react/24/outline';
import { ArticleModal } from '../components/articles/ArticleModal';
import { ArticleTable } from '../components/articles/ArticleTable';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import { SaleModal } from '../components/sale/SaleModal';
import { articleApi, Article } from '../services/articleApi';

export const ArticleListPage: React.FC = () => {
    // Data state
    const [articles, setArticles] = useState<Article[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Pagination
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    const currentItems = articles.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    // Modal states
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingArticle, setEditingArticle] = useState<Article | null>(null);

    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [articleToSave, setArticleToSave] = useState<any | null>(null);

    // Sale modal states
    const [isSaleModalOpen, setIsSaleModalOpen] = useState(false);
    const [sellingArticle, setSellingArticle] = useState<Article | null>(null);

    const [isSaleConfirmOpen, setIsSaleConfirmOpen] = useState(false);
    const [saleToConfirm, setSaleToConfirm] = useState<any | null>(null);

    // Load articles from backend
    const loadArticles = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await articleApi.getAll();
            setArticles(data);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to load articles';
            setError(message);
            console.error('Error loading articles:', err);
        } finally {
            setLoading(false);
        }
    };

    // Load data on mount
    useEffect(() => {
        loadArticles();
    }, []);

    // -----------------------------
    // ADD / EDIT
    // -----------------------------
    const handleOpenAddModal = () => {
        setEditingArticle(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (article: Article) => {
        setEditingArticle(article);
        setIsModalOpen(true);
    };

    const handleOpenDeleteModal = async (article: Article) => {
        if (!window.confirm(`Jeste li sigurni da želite obrisati "${article.name}"?`)) {
            return;
        }

        try {
            setLoading(true);
            await articleApi.delete(article.id);
            setArticles(prev => prev.filter(a => a.id !== article.id));
            alert('Artikl uspješno obrisan!');
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to delete article';
            alert('Greška pri brisanju: ' + message);
            console.error('Error deleting article:', err);
        } finally {
            setLoading(false);
        }
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingArticle(null);
    };

    const handleSaveArticle = (articleData: any) => {
        setArticleToSave(articleData);
        setIsModalOpen(false);
        setIsConfirmOpen(true);
    };

    const handleConfirmSave = async () => {
        if (!articleToSave) return;

        try {
            setLoading(true);

            if (editingArticle) {
                // Update existing article
                const updatedArticle = { ...editingArticle, ...articleToSave };
                await articleApi.update(editingArticle.id, updatedArticle);
                setArticles(prev => prev.map(a => a.id === editingArticle.id ? updatedArticle : a));
                alert('Artikl uspješno ažuriran!');
            } else {
                // Add new article
                const newArticle = await articleApi.create({
                    ...articleToSave,
                    status: 'available',
                    warehouseType: 'main'
                });
                setArticles(prev => [...prev, newArticle]);
                alert('Artikl uspješno dodan!');
            }

            setIsConfirmOpen(false);
            setArticleToSave(null);
            setEditingArticle(null);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to save article';
            alert('Greška pri spremanju: ' + message);
            console.error('Error saving article:', err);
        } finally {
            setLoading(false);
        }
    };

    // -----------------------------
    // SALE LOGIC
    // -----------------------------
    const handleOpenSaleModal = (article: Article) => {
        setSellingArticle(article);
        setIsSaleModalOpen(true);
    };

    const handleCloseSaleModal = () => {
        setIsSaleModalOpen(false);
        setSellingArticle(null);
    };

    const handleSaleArticle = (saleData: any) => {
        if (!sellingArticle) return;

        setSaleToConfirm({ article: sellingArticle, saleData });
        setIsSaleModalOpen(false);
        setIsSaleConfirmOpen(true);
    };

    const handleConfirmSale = async () => {
        if (!saleToConfirm) return;

        const { article, saleData } = saleToConfirm;

        if (article.stock < saleData.quantity) {
            alert('Nedovoljno zaliha!');
            return;
        }

        try {
            setLoading(true);

            // Update article stock
            const updatedArticle = {
                ...article,
                stock: article.stock - saleData.quantity
            };

            await articleApi.update(article.id, updatedArticle);
            setArticles(prev => prev.map(a => a.id === article.id ? updatedArticle : a));

            alert('Prodaja uspješno evidentirana!');
            setIsSaleConfirmOpen(false);
            setSaleToConfirm(null);
            setSellingArticle(null);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to process sale';
            alert('Greška pri prodaji: ' + message);
            console.error('Error processing sale:', err);
        } finally {
            setLoading(false);
        }
    };

    const handleCancelSale = () => {
        setIsSaleConfirmOpen(false);
        setSaleToConfirm(null);
        setSellingArticle(null);
    };

    if (loading && articles.length === 0) {
        return (
            <AppLayout>
                <div className="px-6 py-8 bg-gray-50 min-h-screen flex items-center justify-center">
                    <div className="text-center">
                        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
                        <p className="mt-4 text-gray-600">Učitavanje artikala...</p>
                    </div>
                </div>
            </AppLayout>
        );
    }

    if (error && articles.length === 0) {
        return (
            <AppLayout>
                <div className="px-6 py-8 bg-gray-50 min-h-screen">
                    <div className="bg-red-50 border border-red-200 rounded-md p-4">
                        <p className="text-red-800">Greška: {error}</p>
                        <button
                            onClick={loadArticles}
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
            <div className="px-6 py-8 bg-gray-50 min-h-screen">
                <Header title="Artikli i cjenik" showBackButton={true} />

                {error && (
                    <div className="mt-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
                        <p className="text-yellow-800">Upozorenje: {error}</p>
                    </div>
                )}

                <div className="mt-6 md:flex md:items-center md:justify-between">
                    <div className="flex-1 min-w-0">
                        <div className="flex items-center space-x-4 text-sm text-gray-600">
                            <span>Ukupno artikala: {articles.length}</span>
                            <span className="text-green-600">
                                Dostupno: {articles.filter(a => a.status === 'available').length}
                            </span>
                            <span className="text-red-600">
                                Prodano: {articles.filter(a => a.status === 'sold').length}
                            </span>
                        </div>
                    </div>

                    <div className="mt-4 flex md:mt-0 md:ml-4">
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
                            disabled={loading}
                            className="ml-3 flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <PlusIcon className="h-5 w-5 mr-2" />
                            Novi Artikl
                        </button>
                    </div>
                </div>

                <div className="mt-8 flow-root">
                    <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                        <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                            <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                                {loading && articles.length > 0 && (
                                    <div className="bg-blue-50 px-4 py-2 text-sm text-blue-700">
                                        Obrada...
                                    </div>
                                )}

                                <ArticleTable
                                    data={currentItems}
                                    onEdit={handleOpenEditModal}
                                    onSell={handleOpenSaleModal}
                                    onDelete={handleOpenDeleteModal}
                                />
                            </div>

                            <Pagination
                                currentPage={currentPage}
                                totalCount={articles.length}
                                itemsPerPage={itemsPerPage}
                                onPageChange={setCurrentPage}
                                onItemsPerPageChange={handleItemsPerPageChange}
                            />
                        </div>
                    </div>
                </div>
            </div>

            {/* MODAL: Add/Edit */}
            <ArticleModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSaveArticle}
                initialData={editingArticle}
            />

            {/* MODAL: Sale */}
            <SaleModal
                isOpen={isSaleModalOpen}
                onClose={handleCloseSaleModal}
                onSale={handleSaleArticle}
                article={sellingArticle}
            />

            {/* CONFIRM: Save */}
            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={handleConfirmSave}
                title="Potvrda izmjena"
                message="Jeste li sigurni da želite spremiti promjene?"
            />

            {/* CONFIRM: Sale */}
            <ConfirmDialog
                isOpen={isSaleConfirmOpen}
                onClose={handleCancelSale}
                onConfirm={handleConfirmSale}
                title="Potvrda prodaje"
                message={
                    saleToConfirm
                        ? `Jeste li sigurni da želite prodati "${saleToConfirm.article.name}" u količini ${saleToConfirm.saleData.quantity} kom?`
                        : 'Jeste li sigurni da želite izvršiti prodaju?'
                }
            />
        </AppLayout>
    );
};
