import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { Article } from '../../types/article';

interface ArticleModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (article: Article) => void;
  initialData: Article | null;
}

const defaultArticle: Article = {
  id: '',
  name: '',
  description: '',
  purchasePrice: 0,
  retailPrice: 0,
  taxRate: 25,
  stock: 0,
  unitOfMeasureCode: 'kom',
  supplierName: '',
  status: 'available',
  warehouseType: 'main',
  createdAt: new Date().toISOString(),
  updatedAt: new Date().toISOString()
};

export const ArticleModal: React.FC<ArticleModalProps> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState<Article>(defaultArticle);

  useEffect(() => {
    // Ako dobijemo podatke za uređivanje, popuni formu
    if (initialData) {
      setFormData(initialData);
    } else {
      // Inače, resetiraj na praznu formu (za novi unos)
      setFormData(defaultArticle);
    }
  }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };
  
  const handleSave = () => {
    onSave(formData);
  };

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
              <Dialog.Title className="text-xl font-semibold text-gray-900">
                {initialData ? 'Uređivanje artikla' : 'Novi artikl'}
              </Dialog.Title>

              <div className="mt-6 grid grid-cols-1 gap-y-6 sm:grid-cols-2 sm:gap-x-4">
                {/* Ovdje idu polja forme */}
                <div>
                  <label htmlFor="name" className="block text-sm font-medium text-gray-700">Naziv</label>
                  <input type="text" name="name" id="name" value={formData.name} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                </div>
                <div>
                  <label htmlFor="id" className="block text-sm font-medium text-gray-700">Oznaka (ID)</label>
                  <input type="text" name="id" id="id" value={formData.id} onChange={handleChange} disabled={!!initialData} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm disabled:bg-gray-100 sm:text-sm" />
                </div>
                <div>
                  <label htmlFor="retailPrice" className="block text-sm font-medium text-gray-700">MPC (€)</label>
                  <input type="number" name="retailPrice" id="retailPrice" value={formData.retailPrice} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                </div>
                <div>
                  <label htmlFor="stock" className="block text-sm font-medium text-gray-700">Stanje</label>
                  <input type="number" name="stock" id="stock" value={formData.stock} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                </div>
                {/* ... Dodajte i ostala polja po potrebi ... */}
              </div>

              <div className="mt-8 flex justify-end space-x-3">
                <button type="button" onClick={onClose} className="rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
                <button type="button" onClick={handleSave} className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi</button>
              </div>
            </Dialog.Panel>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};