import React from 'react';
import { PencilSquareIcon, TrashIcon, TicketIcon, CurrencyDollarIcon } from '@heroicons/react/24/outline';
import { Client } from '../../types/client';

interface ClientTableProps {
    data: Client[];
    onEdit: (client: Client) => void;
    onPledge: (client: Client) => void;
    onPurchase: (client: Client) => void;
}

export const ClientTable: React.FC<ClientTableProps> = ({ data, onEdit, onPledge, onPurchase }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">ID</th>
                    
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Naziv</th>
                    
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Adresa</th>
                    
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Mjesto</th>

                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">OIB</th>

                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Status</th>
                    
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">
                      <span className="sr-only">Akcije</span>
                    </th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.map((client) => (
                    <tr key={client.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{client.id}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{client.name}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{client.address}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{client.city}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{client.taxId}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">
                            <span className={`inline-flex items-center rounded-md px-2 py-1 text-xs font-medium ${
                                client.status === 'active' 
                                ? 'bg-green-50 text-green-700 ring-1 ring-inset ring-green-600/20' 
                                : 'bg-yellow-50 text-yellow-800 ring-1 ring-inset ring-yellow-600/20'
                            }`}>
                                {client.status === 'active' ? 'Aktivan' : 'Neaktivan'}
                            </span>
                        </td>
                        
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-right text-sm font-medium border-l border-gray-200">
                             {/* Gumb za Uredi */}
                             <div className="relative inline-block group ml-2">
                                <button onClick={() => onEdit(client)} className="text-indigo-600 hover:text-indigo-900">
                                    <PencilSquareIcon className="h-5 w-5" />
                                </button>
                                <span className="absolute left-1/2 -translate-x-1/2 -top-full mt-2 hidden group-hover:block bg-gray-700 text-white text-xs rounded py-1 px-2 whitespace-nowrap">Uredi</span>
                            </div>

                             {/* Gumb za Otkup */}
                             <div className="relative inline-block group ml-2">
                                <button onClick={() => onPurchase(client)} className="text-gray-500 hover:text-green-600">
                                    <CurrencyDollarIcon className="h-5 w-5" />
                                </button>
                                <span className="absolute left-1/2 -translate-x-1/2 -top-full mt-2 hidden group-hover:block bg-gray-700 text-white text-xs rounded py-1 px-2 whitespace-nowrap">Novi otkup</span>
                            </div>

                             {/* Gumb za Zalog */}
                            <div className="relative inline-block group ml-2">
                                <button onClick={() => onPledge(client)} className="text-gray-500 hover:text-blue-600">
                                    <TicketIcon className="h-5 w-5" />
                                </button>
                                <span className="absolute left-1/2 -translate-x-1/2 -top-full mt-2 hidden group-hover:block bg-gray-700 text-white text-xs rounded py-1 px-2 whitespace-nowrap">Novi zalog</span>
                            </div>
                        
                            <button className="ml-4 text-red-600 hover:text-red-900">
                                <TrashIcon className="h-5 w-5" />
                                <span className="sr-only">, {client.name}</span>
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};