import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { Article, SaleData } from '../../types/article';
import { Customer } from '../../types/Customer';
import * as customerApi from '../../services/customerApi';

// Export this interface so ArticleListPage can use it
export interface SaleModalData {
  quantity: number;
  pricePerUnit: number;
  customerName: string;
  customerId?: string;
}

interface SaleModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSale: (saleData: SaleData) => void;
  article: Article | null;
}

export const SaleModal: React.FC<SaleModalProps> = ({ isOpen, onClose, onSale, article }) => {
  const [quantity, setQuantity] = useState<number>(1);
  const [pricePerUnit, setPricePerUnit] = useState<number>(0);
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>('');
  const [customCustomerName, setCustomCustomerName] = useState<string>('');
  const [useCustomCustomer, setUseCustomCustomer] = useState<boolean>(false);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [loadingCustomers, setLoadingCustomers] = useState<boolean>(false);

  useEffect(() => {
    if (article && isOpen) {
      // Postavi zadanu prodajnu cijenu na MPC artikla
      setPricePerUnit(article.retailPrice);
      setQuantity(1);
      // Resetiraj ostala polja
      setSelectedCustomerId('');
      setCustomCustomerName('');
      setUseCustomCustomer(false);
    }
  }, [article, isOpen]);

  useEffect(() => {
    if (isOpen) {
      loadCustomers();
    }
  }, [isOpen]);

  const loadCustomers = async () => {
    try {
      setLoadingCustomers(true);
      const data = await customerApi.getAll();
      setCustomers(data);
    } catch (error) {
      console.error('Failed to load customers:', error);
    } finally {
      setLoadingCustomers(false);
    }
  };

  const handleSale = () => {
    if (!article) return;

    const customerName = useCustomCustomer ? customCustomerName :
      customers.find(c => c.id === selectedCustomerId)?.fullName || '';

    if (!customerName.trim()) {
      alert('Molimo unesite ime kupca');
      return;
    }

    if (pricePerUnit <= 0) {
      alert('Molimo unesite valjanu prodajnu cijenu');
      return;
    }

    if (quantity <= 0 || quantity > article.stock) {
      alert('Molimo unesite valjanu količinu');
      return;
    }

    const saleData: SaleData = {
      quantity,
      pricePerUnit,
      customerName: customerName.trim(),
      customerId: useCustomCustomer ? undefined : selectedCustomerId,
      articleId: '',
      totalPrice: 0,
      salePrice: 0,
      saleDate: ''
    };

    onSale(saleData);
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);
  };

  const totalPrice = quantity * pricePerUnit;

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
              <Dialog.Title className="text-xl font-semibold text-gray-900">
                Prodaja artikla
              </Dialog.Title>

              {article && (
                <div className="mt-4 p-4 bg-gray-50 rounded-lg">
                  <h4 className="font-medium text-gray-900">{article.name}</h4>
                  <p className="text-sm text-gray-600">Oznaka: {article.id}</p>
                  <p className="text-sm text-gray-600">MPC: {formatCurrency(article.retailPrice)}</p>
                  <p className="text-sm text-gray-600">Stanje: {article.stock} {article.unitOfMeasureCode}</p>
                </div>
              )}

              <div className="mt-6 space-y-4">
                <div>
                  <label htmlFor="quantity" className="block text-sm font-medium text-gray-700">
                    Količina
                  </label>
                  <input
                    type="number"
                    name="quantity"
                    id="quantity"
                    min="1"
                    max={article?.stock || 1}
                    value={quantity}
                    onChange={(e) => setQuantity(parseInt(e.target.value) || 1)}
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  />
                </div>

                <div>
                  <label htmlFor="pricePerUnit" className="block text-sm font-medium text-gray-700">
                    Cijena po jedinici (€)
                  </label>
                  <input
                    type="number"
                    name="pricePerUnit"
                    id="pricePerUnit"
                    step="0.01"
                    min="0"
                    value={pricePerUnit}
                    onChange={(e) => setPricePerUnit(parseFloat(e.target.value) || 0)}
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  />
                </div>

                <div className="p-3 bg-indigo-50 rounded-md">
                  <p className="text-sm font-medium text-gray-700">
                    Ukupna cijena: <span className="text-lg font-semibold text-indigo-600">{formatCurrency(totalPrice)}</span>
                  </p>
                </div>

                <div>
                  <div className="flex items-center mb-3">
                    <input
                      id="existing-customer"
                      name="customer-type"
                      type="radio"
                      checked={!useCustomCustomer}
                      onChange={() => setUseCustomCustomer(false)}
                      className="h-4 w-4 text-indigo-600 border-gray-300 focus:ring-indigo-500"
                    />
                    <label htmlFor="existing-customer" className="ml-2 block text-sm font-medium text-gray-700">
                      Odaberite postojećeg kupca
                    </label>
                  </div>
                  
                  {!useCustomCustomer && (
                    <select
                      value={selectedCustomerId}
                      onChange={(e) => setSelectedCustomerId(e.target.value)}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                      disabled={loadingCustomers}
                    >
                      <option value="">
                        {loadingCustomers ? '-- Učitavanje kupaca... --' : '-- Odaberite kupca --'}
                      </option>
                      {customers.map((customer) => (
                        <option key={customer.id} value={customer.id}>
                          {customer.fullName}
                        </option>
                      ))}
                    </select>
                  )}

                  <div className="flex items-center mt-3 mb-3">
                    <input
                      id="custom-customer"
                      name="customer-type"
                      type="radio"
                      checked={useCustomCustomer}
                      onChange={() => setUseCustomCustomer(true)}
                      className="h-4 w-4 text-indigo-600 border-gray-300 focus:ring-indigo-500"
                    />
                    <label htmlFor="custom-customer" className="ml-2 block text-sm font-medium text-gray-700">
                      Unesite ime novog kupca
                    </label>
                  </div>

                  {useCustomCustomer && (
                    <input
                      type="text"
                      placeholder="Ime kupca"
                      value={customCustomerName}
                      onChange={(e) => setCustomCustomerName(e.target.value)}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                    />
                  )}
                </div>
              </div>

              <div className="mt-8 flex justify-end space-x-3">
                <button
                  type="button"
                  onClick={onClose}
                  className="rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                >
                  Odustani
                </button>
                <button
                  type="button"
                  onClick={handleSale}
                  className="rounded-md bg-green-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-green-700"
                >
                  Prodaj
                </button>
              </div>
            </Dialog.Panel>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};