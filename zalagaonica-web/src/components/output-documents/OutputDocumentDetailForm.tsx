import React, { useState, useEffect } from 'react';
import { OutputDocument } from '../../types/outputDocument';

interface OutputDocumentDetailFormProps {
  document: OutputDocument | null;
  onSave: (updatedDocument: OutputDocument) => void;
  onCancel: () => void;
}

const defaultDocument: OutputDocument = {
    id: '',
    type: 'individual',
    documentDate: new Date().toISOString().split('T')[0],
    customerName: '',
    totalValue: 0,
    status: 'draft',
    note: '',
    year: new Date().getFullYear(),
    warehouseName: 'Glavno Skladište',
    documentNumber: '',
    clientName: '',
    documentType: '',
    taxAmount: 0,
    totalWithTax: 0,
    operator: '',
    isPosted: false,
    paymentType: 'cash',
    isPaid: false,
    currency: '',
    items: [],
    totalRetailPrice: 0,
    totalTaxAmount: 0,
    totalWithoutTax: 0,
};

export const OutputDocumentDetailForm: React.FC<OutputDocumentDetailFormProps> = ({ document, onSave, onCancel }) => {
  const [formData, setFormData] = useState<OutputDocument>(defaultDocument);

  useEffect(() => {
    setFormData(document || defaultDocument);
  }, [document]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    if (type === 'checkbox') {
        const checked = (e.target as HTMLInputElement).checked;
        setFormData(prev => ({ ...prev, [name]: checked }));
    } else {
        const isNumber = ['number'].includes(type);
        setFormData(prev => ({ ...prev, [name]: isNumber ? parseFloat(value) || 0 : value }));
    }
  };
  
  const handleSave = () => {
      onSave(formData);
  };
  
  return (
    <div className="rounded-lg bg-white shadow p-6">
      <h2 className="text-xl font-semibold mb-4 text-gray-800">{document ? 'Uredi Izlazni Dokument' : 'Novi Izlazni Dokument'}</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Lijeva kolona: Opći podaci */}
        <div className="space-y-4">
          <div>
            <label htmlFor="documentNumber" className="block text-sm font-medium text-gray-700">Broj dokumenta</label>
            <input type="text" name="documentNumber" id="documentNumber" value={formData.documentNumber} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label htmlFor="documentDate" className="block text-sm font-medium text-gray-700">Datum dokumenta</label>
            <input type="date" name="documentDate" id="documentDate" value={formData.documentDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label htmlFor="clientName" className="block text-sm font-medium text-gray-700">Naziv klijenta</label>
            <input type="text" name="clientName" id="clientName" value={formData.clientName} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label htmlFor="documentType" className="block text-sm font-medium text-gray-700">Tip dokumenta</label>
            <input type="text" name="documentType" id="documentType" value={formData.documentType} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label htmlFor="warehouseName" className="block text-sm font-medium text-gray-700">Skladište</label>
            <input type="text" name="warehouseName" id="warehouseName" value={formData.warehouseName} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
        </div>

        {/* Desna kolona: Financijski podaci */}
        <div className="space-y-4">
          {/* Dodana polja iz GUI-a */}
          <div>
              <label htmlFor="discountPercentage" className="block text-sm font-medium text-gray-700">Rabat (%)</label>
              <input type="number" name="discountPercentage" id="discountPercentage" value={formData.discountPercentage} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
              <label htmlFor="totalRetailPrice" className="block text-sm font-medium text-gray-700">Ukupno MPC</label>
              <input type="number" name="totalRetailPrice" id="totalRetailPrice" value={formData.totalRetailPrice} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
              <label htmlFor="totalTaxAmount" className="block text-sm font-medium text-gray-700">Ukupno Porez</label>
              <input type="number" name="totalTaxAmount" id="totalTaxAmount" value={formData.totalTaxAmount} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
              <label htmlFor="totalWithoutTax" className="block text-sm font-medium text-gray-700">Ukupno bez poreza</label>
              <input type="number" name="totalWithoutTax" id="totalWithoutTax" value={formData.totalWithoutTax} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label htmlFor="status" className="block text-sm font-medium text-gray-700">Status</label>
            <select name="status" id="status" value={formData.status} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
              <option value="draft">Nacrt</option>
              <option value="posted">Proknjižen</option>
              <option value="canceled">Poništen</option>
            </select>
          </div>
          <div>
            <label htmlFor="isPosted" className="block text-sm font-medium text-gray-700">Proknjiženo?</label>
            <input type="checkbox" name="isPosted" id="isPosted" checked={formData.isPosted} onChange={handleChange} className="mt-1 rounded border-gray-300 text-indigo-600 shadow-sm focus:ring-indigo-500" />
          </div>
        </div>
      </div>
      
      <div className="mt-8 flex justify-end gap-x-3 border-t pt-6">
          <button type="button" onClick={onCancel} className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
          <button type="button" onClick={handleSave} className="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi</button>
      </div>
    </div>
  );
};