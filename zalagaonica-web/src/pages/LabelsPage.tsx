import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { TicketIcon, PlusIcon, PrinterIcon, DocumentDuplicateIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { labelApi, Label } from '../services/labelApi';

export const LabelsPage: React.FC = () => {
  const [labels, setLabels] = useState<Label[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadLabels();
  }, []);

  const loadLabels = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await labelApi.getAll();
      setLabels(data);
    } catch (err: any) {
      console.error('Error loading labels:', err);
      setError(err.message || 'Greška pri učitavanju naljepnica');
    } finally {
      setLoading(false);
    }
  };

  const handlePrint = async (label: Label) => {
    try {
      await labelApi.markAsPrinted(label.id);
      alert(`Ispis naljepnice za: ${label.articleName}`);
      await loadLabels();
    } catch (err: any) {
      alert('Greška pri ispisu: ' + (err.message || 'Nepoznata greška'));
    }
  };

  const handlePrintAll = () => {
    alert(`Ispis svih naljepnica (${labels.length})`);
  };

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje naljepnica..." />
      </AppLayout>
    );
  }

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {error && (
          <div className="mb-4 bg-red-50 border-l-4 border-red-400 p-4">
            <div className="flex">
              <ExclamationCircleIcon className="h-5 w-5 text-red-400 mr-2" />
              <div>
                <p className="text-sm text-red-700">{error}</p>
                <button onClick={loadLabels} className="text-sm text-red-600 underline mt-1">
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <TicketIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Naljepnice
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Generiranje i ispis naljepnica za artikle
            </p>
          </div>
          <div className="flex space-x-2">
            <button
              onClick={handlePrintAll}
              className="flex items-center px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
            >
              <PrinterIcon className="h-5 w-5 mr-2" />
              Ispiši sve
            </button>
            <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
              <PlusIcon className="h-5 w-5 mr-2" />
              Nova naljepnica
            </button>
          </div>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Šifra artikla
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Naziv artikla
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Cijena
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Barkod
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Količina
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {labels.map((label) => (
                <tr key={label.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {label.articleCode}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {label.articleName}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {label.price.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 font-mono">
                    {label.barcode}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {label.quantity} kom
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center items-center space-x-2">
                      <button
                        onClick={() => handlePrint(label)}
                        className="text-indigo-600 hover:text-indigo-900"
                        title="Ispiši"
                      >
                        <PrinterIcon className="h-5 w-5" />
                      </button>
                      <button className="text-green-600 hover:text-green-900" title="Dupliciraj">
                        <DocumentDuplicateIcon className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {labels.length === 0 && (
                <tr>
                  <td colSpan={6} className="text-center py-8 text-gray-500">
                    Nema zapisa o naljepnicama.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </AppLayout>
  );
};

export default LabelsPage;
