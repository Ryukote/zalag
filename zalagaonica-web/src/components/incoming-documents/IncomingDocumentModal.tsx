import React, { Fragment, useEffect, useState } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { IncomingDocument } from '../../types/incomingDocument';
import { IncomingDocumentDetailForm } from './DocumentDetailForm';

interface IncomingDocumentModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (document: IncomingDocument) => void;
  initialData: IncomingDocument | null;
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
  // Polja koja su nedostajala
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

export const IncomingDocumentModal: React.FC<IncomingDocumentModalProps> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState<IncomingDocument>(defaultDocument);

  useEffect(() => { setFormData(initialData || defaultDocument); }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    // ... implementirajte handleChange logiku
  };

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-4xl rounded-lg bg-white p-6 shadow-xl">
              <h3 className="text-xl font-semibold text-gray-900">{initialData ? 'Uređivanje dokumenta' : 'Novi ulazni dokument'}</h3>
              <div className="mt-6 grid grid-cols-1 lg:grid-cols-2 gap-x-8 gap-y-4">
                {/* Lijevi panel */}
                <div className="space-y-4">
                  {/* ... Ovdje idu sva polja iz lijevog panela forme ... */}
                  <div>
                    <label>Dobavljač</label>
                    <input name="supplierName" value={formData.supplierName} onChange={handleChange} />
                  </div>
                  {/* Itd. za ostala polja: datumi, brojevi, napomena, checkbox... */}
                </div>
                {/* Desni panel */}
                <div className="bg-gray-50 p-4 rounded-md grid grid-cols-2 gap-4 h-fit">
                   {/* ... Ovdje idu sva polja iz desnog, financijskog panela ... */}
                   <div>
                    <label>Fakturna vrijednost</label>
                    <input name="invoiceValue" type="number" value={formData.invoiceValue} onChange={handleChange} />
                  </div>
                  {/* Itd. za nabavnu, maloprodajnu, maržu... */}
                </div>
              </div>
              <div className="mt-8 flex justify-end gap-x-3 border-t pt-6">
                <button type="button" onClick={onClose} className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
                <button type="button" onClick={() => onSave(formData)} className="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi</button>
              </div>
            </Dialog.Panel>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};