import React from 'react';
import { Article } from '../../types/article';
import { PencilSquareIcon, TrashIcon, CurrencyEuroIcon } from '@heroicons/react/24/outline';

interface ArticleTableProps {
    data: Article[];
    onEdit: (article: Article) => void;
    onSell: (article: Article) => void;
    onDelete: (article: Article) => void;
}

const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);
};

const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('hr-HR');
};

export const ArticleTable: React.FC<ArticleTableProps> = ({ data, onEdit, onSell, onDelete }) => {
        
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">Status</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Oznaka</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Naziv</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Stanje</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">MPC</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Prodajna cijena</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Dobavljač</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Prodaja</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200">
                        <span className="sr-only">Akcije</span>
                    </th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.map((article) => (
                    <tr key={article.id} className={`hover:bg-gray-50 ${article.status === 'sold' ? 'bg-red-50' : ''}`}>
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm">
                            {article.status === 'sold' ? (
                                <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">
                                    Prodano
                                </span>
                            ) : (
                                <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">
                                    Dostupno
                                </span>
                            )}
                        </td>
                        <td className="whitespace-nowrap py-4 px-6 text-sm font-medium text-gray-900 border-l border-gray-200">{article.id}</td>
                        <td className="px-6 py-4 text-sm font-semibold text-gray-800 border-l border-gray-200">{article.name}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{`${article.stock} ${article.unitOfMeasureCode}`}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(article.retailPrice)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{article.supplierName}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">
                            {article.status === 'sold' && article.saleInfoPrice ? (
                                <div className="text-xs">
                                    <div className="font-medium text-gray-900">{formatCurrency(article.saleInfoPrice)}</div>
                                    <div className="text-gray-500">{article.saleInfoDate ? formatDate(article.saleInfoDate) : '-'}</div>
                                    <div className="text-gray-500 truncate max-w-24" title={article.saleInfoCustomerName}>
                                        {article.saleInfoCustomerName || '-'}
                                    </div>
                                </div>
                            ) : (
                                <span className="text-gray-400">-</span>
                            )}
                        </td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-right text-sm font-medium border-l border-gray-200">
                            {article.status === 'available' && (
                                <>
                                    <button 
                                        onClick={() => onSell(article)} 
                                        className="text-green-600 hover:text-green-900 mr-4"
                                        title={`Prodaj ${article.name}`}
                                    >
                                        <CurrencyEuroIcon className="h-5 w-5" />
                                        <span className="sr-only">Prodaj {article.name}</span>
                                    </button>
                                    
                                    <button onClick={() => onEdit(article)} className="text-indigo-600 hover:text-indigo-900 mr-4">
                                        <PencilSquareIcon className="h-5 w-5" />
                                        <span className="sr-only">Uredi {article.name}</span>
                                    </button>
                                </>
                            )}

                            <button className="text-red-600 hover:text-red-900" onClick={() => onDelete(article)}>
                                <TrashIcon className="h-5 w-5" />
                                <span className="sr-only">Obriši {article.name}</span>
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};