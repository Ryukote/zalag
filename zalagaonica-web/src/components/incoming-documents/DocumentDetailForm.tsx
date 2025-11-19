import React, { useState, useEffect } from 'react';
import { IncomingDocument } from '../../types/incomingDocument';

interface IncomingDocumentDetailFormProps {
  document: IncomingDocument | null;
  onSave: (updatedDocument: IncomingDocument) => void;
  onCancel: () => void;
}

const defaultDocument: IncomingDocument = {
  id: 0,
  supplierName: '',
  bookingDate: new Date().toISOString().split('T')[0],
  documentNumber: '',
  documentDate: new Date().toISOString().split('T')[0],
  purchaseValue: 0,
  margin: 0,
  tax: 0,
  status: 'otvoren',
  warehouseName: 'Zalagaonica (ZG3)',
  documentType: 'PRIMKA',
  year: new Date().getFullYear(),
  operator: '',
  dueDate: '',
  isPosted: false,
  invoiceValue: 0,
  discount: 0,
  cost: 0,
  wholesaleValue: 0,
  vatAmount: 0,
  retailValue: 0,
  returnFee: 0,
  totalWithReturnFee: 0,
  pretaxAmount: 0,
  totalPaid: 0,
  note: '',
};

export const IncomingDocumentDetailForm: React.FC<IncomingDocumentDetailFormProps> = ({ document, onSave, onCancel }) => {
  const [formData, setFormData] = useState<IncomingDocument>(defaultDocument);

  useEffect(() => {
    setFormData(document || defaultDocument);
  }, [document]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    const isCheckbox = type === 'checkbox';
    const isChecked = isCheckbox ? (e.target as HTMLInputElement).checked : undefined;
    const isNumber = type === 'number';

    setFormData(prevData => ({
        ...prevData,
        [name]: isCheckbox ? isChecked : isNumber ? parseFloat(value) || 0 : value,
    }));
  };

  return (
    <div className="p-6 bg-white rounded-lg">
      <h3 className="text-xl font-semibold text-gray-900 mb-6">
        {document ? `Uređivanje dokumenta: ${document.documentNumber}` : 'Novi ulazni dokument'}
      </h3>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-x-8 gap-y-4">
        
        {/* --- LIJEVI PANEL --- */}
        <div className="space-y-4">
          <div className="grid grid-cols-3 gap-4">
            <div>
              <label htmlFor="id" className="block text-sm font-medium text-gray-700">Broj</label>
              <input type="text" name="id" id="id" value={formData.id} disabled className="mt-1 block w-full rounded-md border-gray-300 shadow-sm bg-gray-100 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="year" className="block text-sm font-medium text-gray-700">Godina</label>
              <input type="number" name="year" id="year" value={formData.year} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="operator" className="block text-sm font-medium text-gray-700">Operator</label>
              <input type="text" name="operator" id="operator" value={formData.operator} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
          </div>
          <div>
            <label htmlFor="supplierName" className="block text-sm font-medium text-gray-700">Naziv Komitenta</label>
            <input type="text" name="supplierName" id="supplierName" value={formData.supplierName} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label htmlFor="bookingDate" className="block text-sm font-medium text-gray-700">Datum knjiženja</label>
              <input type="date" name="bookingDate" id="bookingDate" value={formData.bookingDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
             <div>
              <label htmlFor="documentDate" className="block text-sm font-medium text-gray-700">Datum dokumenta</label>
              <input type="date" name="documentDate" id="documentDate" value={formData.documentDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
          </div>
           <div>
              <label htmlFor="documentNumber" className="block text-sm font-medium text-gray-700">Ulazni dokument - broj</label>
              <input type="text" name="documentNumber" id="documentNumber" value={formData.documentNumber} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
          <div>
            <label htmlFor="note" className="block text-sm font-medium text-gray-700">Napomena</label>
            <textarea name="note" id="note" value={formData.note || ''} onChange={handleChange} rows={3} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
          </div>
          <div className="flex items-center">
              <input type="checkbox" name="isPosted" id="isPosted" checked={formData.isPosted} onChange={handleChange} className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-600"/>
              <label htmlFor="isPosted" className="ml-2 block text-sm text-gray-900">Proknjižen i zaključen</label>
          </div>
        </div>

        {/* --- DESNI PANEL --- */}
        <div className="bg-gray-50 p-4 rounded-md grid grid-cols-2 gap-4 h-fit">
            <div>
              <label htmlFor="invoiceValue" className="block text-sm font-medium text-gray-700">Fakturna vrijednost</label>
              <input type="number" name="invoiceValue" id="invoiceValue" value={formData.invoiceValue} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
             <div>
              <label htmlFor="discount" className="block text-sm font-medium text-gray-700">Rabat</label>
              <input type="number" name="discount" id="discount" value={formData.discount} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
             <div>
              <label htmlFor="cost" className="block text-sm font-medium text-gray-700">Trošak</label>
              <input type="number" name="cost" id="cost" value={formData.cost} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="purchaseValue" className="block text-sm font-medium text-gray-700">Nabavna vrijednost</label>
              <input type="number" name="purchaseValue" id="purchaseValue" value={formData.purchaseValue} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="retailValue" className="block text-sm font-medium text-gray-700">Maloprodajna vr.</label>
              <input type="number" name="retailValue" id="retailValue" value={formData.retailValue} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="margin" className="block text-sm font-medium text-gray-700">Marža</label>
              <input type="number" name="margin" id="margin" value={formData.margin} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
            <div>
              <label htmlFor="vatAmount" className="block text-sm font-medium text-gray-700">Porez na dod. vr.</label>
              <input type="number" name="vatAmount" id="vatAmount" value={formData.vatAmount} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
             <div>
              <label htmlFor="totalPaid" className="block text-sm font-medium text-gray-700">Ukupno plaćeno</label>
              <input type="number" name="totalPaid" id="totalPaid" value={formData.totalPaid} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"/>
            </div>
        </div>
      </div>
      <div className="mt-8 flex justify-end gap-x-3 border-t pt-6">
          <button type="button" onClick={onCancel} className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
          <button type="button" onClick={() => onSave(formData)} className="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi dokument</button>
      </div>
    </div>
  );
};