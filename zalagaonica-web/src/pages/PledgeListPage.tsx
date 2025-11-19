import React, { useState, useEffect } from 'react';
import { MagnifyingGlassIcon, PencilIcon, TrashIcon, EyeIcon, DocumentTextIcon, CheckCircleIcon, ShoppingCartIcon } from '@heroicons/react/24/outline';
import Header from '../components/layout/Header';
import { pledgeApi } from '../services/pledgeApi';
import { clientApi, Client } from '../services/clientApi';
import * as PdfReportsApi from '../services/pdfReportsApi';

// Helper to convert API pledge dates
interface Pledge {
  id: string;
  clientId: string;
  clientName: string;
  itemName: string;
  itemDescription: string;
  estimatedValue: number;
  loanAmount: number;
  returnAmount: number;
  period: number;
  pledgeDate: Date;
  redeemDeadline: Date;
  redeemed: boolean;
  forfeited: boolean;
  itemImagesJson: string;
  warrantyFilesJson: string;
}

const PledgeListPage: React.FC = () => {
  // Data state
  const [pledges, setPledges] = useState<Pledge[]>([]);
  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [editing, setEditing] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<'svi' | 'aktivan' | 'isplaćen' | 'istekao'>('svi');

  // Load data from backend
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [pledgesData, clientsData] = await Promise.all([
        pledgeApi.getAll(),
        clientApi.getAll()
      ]);

      // Convert API pledges to local format
      const convertedPledges = pledgesData.map(p => ({
        ...p,
        pledgeDate: new Date(p.pledgeDate),
        redeemDeadline: new Date(p.redeemDeadline)
      }));

      setPledges(convertedPledges);
      setClients(clientsData);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to load data';
      setError(message);
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  // Load data on mount
  useEffect(() => {
    loadData();
  }, []);

  const filteredPledges = pledges
    .filter(p => statusFilter === 'svi' || (p.redeemed ? 'isplaćen' : 'aktivan') === statusFilter)
    .filter(p => p.clientName.toLowerCase().includes(searchTerm.toLowerCase()));

  const handleDelete = async (id: string) => {
    if (!window.confirm('Želite li obrisati ovaj zalog?')) return;

    try {
      setLoading(true);
      await pledgeApi.delete(id);
      setPledges(prev => prev.filter(p => p.id !== id));
      alert('Zalog uspješno obrisan!');
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to delete pledge';
      alert('Greška pri brisanju: ' + message);
      console.error('Error deleting pledge:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (id: string) => {
    setEditing(id);
  };

  const handleGeneratePledgeAgreement = async (pledgeId: string) => {
    try {
      const pledge = pledges.find(p => p.id === pledgeId);
      if (!pledge) {
        alert('Zalog nije pronađen');
        return;
      }

      const client = clients.find(c => c.id === pledge.clientId);

      const pledgeAgreementData: PdfReportsApi.PledgeAgreementData = {
        pledgeNumber: pledge.id,
        pledgeDate: pledge.pledgeDate,
        client: {
          name: pledge.clientName,
          address: client?.address,
          city: client?.city,
          oib: client?.idCardNumber
        },
        item: {
          name: pledge.itemName,
          description: pledge.itemDescription,
          estimatedValue: pledge.estimatedValue
        },
        loanAmount: pledge.loanAmount,
        returnAmount: pledge.returnAmount,
        period: pledge.period,
        redeemDeadline: pledge.redeemDeadline
      };

      await PdfReportsApi.generatePledgeAgreement(pledgeAgreementData);
    } catch (error) {
      console.error('Error generating PDF:', error);
      alert(`Došlo je do greške pri generiranju PDF-a: ${error instanceof Error ? error.message : 'Nepoznata greška'}`);
    }
  };

  const handleRedeem = async (id: string) => {
    if (!window.confirm('Je li klijent isplatio zalog i preuzeo predmet?')) return;

    try {
      setLoading(true);
      await pledgeApi.redeem(id);
      setPledges(prev => prev.map(p => p.id === id ? { ...p, redeemed: true } : p));
      alert('Zalog uspješno isplaćen!');
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to redeem pledge';
      alert('Greška pri isplati: ' + message);
      console.error('Error redeeming pledge:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleForfeit = async (id: string) => {
    if (!window.confirm('Prenijeti predmet u glavni skladište za prodaju? Ova akcija ne može biti poništena.')) return;

    try {
      setLoading(true);
      await pledgeApi.forfeit(id);
      setPledges(prev => prev.map(p => p.id === id ? { ...p, forfeited: true } : p));
      alert('Predmet uspješno preuzet u vlasništvo i dodan u skladište!');
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to forfeit pledge';
      alert('Greška pri preuzimanju: ' + message);
      console.error('Error forfeiting pledge:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdatePledge = async (updatedPledge: Pledge) => {
    try {
      setLoading(true);

      // Convert to DTO for API
      const pledgeDto = {
        id: updatedPledge.id,
        clientId: updatedPledge.clientId,
        clientName: updatedPledge.clientName,
        itemName: updatedPledge.itemName,
        itemDescription: updatedPledge.itemDescription,
        estimatedValue: updatedPledge.estimatedValue,
        loanAmount: updatedPledge.loanAmount,
        returnAmount: updatedPledge.returnAmount,
        period: updatedPledge.period,
        pledgeDate: updatedPledge.pledgeDate.toISOString(),
        redeemDeadline: updatedPledge.redeemDeadline.toISOString(),
        itemImagesJson: updatedPledge.itemImagesJson,
        warrantyFilesJson: updatedPledge.warrantyFilesJson
      };

      await pledgeApi.update(updatedPledge.id, pledgeDto);
      setPledges(prev => prev.map(p => p.id === updatedPledge.id ? updatedPledge : p));
      alert('Zalog uspješno ažuriran!');
      setEditing(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to update pledge';
      alert('Greška pri ažuriranju: ' + message);
      console.error('Error updating pledge:', err);
    } finally {
      setLoading(false);
    }
  };

  if (editing) {
    const pledge = pledges.find(p => p.id === editing);
    if (!pledge) return null;

    return (
      <div className="px-6 py-8 bg-gray-50 min-h-screen">
        <Header title={`Uređivanje zaloga: ${pledge.id}`} showBackButton={true} />
        <div className="max-w-2xl mx-auto bg-white p-6 rounded-lg shadow">
          <h2 className="text-xl font-semibold mb-4">Uredi podatke o zalogu</h2>
          <label className="block text-sm font-medium text-gray-700">Iznos pozajmice (€)</label>
          <input
            type="number"
            value={pledge.loanAmount}
            onChange={e => {
              const updatedPledge = { ...pledge, loanAmount: parseFloat(e.target.value) };
              setPledges(prev => prev.map(p => p.id === pledge.id ? updatedPledge : p));
            }}
            className="block w-full mt-1 rounded-md border-gray-300 shadow-sm"
          />
          <button
            onClick={() => handleUpdatePledge(pledge)}
            disabled={loading}
            className="mt-4 bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Spremi
          </button>
        </div>
      </div>
    );
  }

  if (loading && pledges.length === 0) {
    return (
      <div className="px-6 py-8 bg-gray-50 min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Učitavanje zaloga...</p>
        </div>
      </div>
    );
  }

  if (error && pledges.length === 0) {
    return (
      <div className="px-6 py-8 bg-gray-50 min-h-screen">
        <div className="bg-red-50 border border-red-200 rounded-md p-4">
          <p className="text-red-800">Greška: {error}</p>
          <button
            onClick={loadData}
            className="mt-2 text-sm text-red-600 hover:text-red-800 underline"
          >
            Pokušaj ponovno
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="px-6 py-8 bg-gray-50 min-h-screen">
      <Header title="Knjiga Zaloga" showBackButton={true} />

      {error && (
        <div className="mt-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
          <p className="text-yellow-800">Upozorenje: {error}</p>
        </div>
      )}

      <div className="mt-6 md:flex md:items-center md:justify-between">
        <div className="relative rounded-md shadow-sm">
          <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
            <MagnifyingGlassIcon className="h-5 w-5 text-gray-400" />
          </div>
          <input
            type="search"
            className="block w-full rounded-md border-0 py-2 pl-10 text-gray-900 ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-indigo-600 sm:text-sm"
            placeholder="Pretraži po imenu klijenta..."
            value={searchTerm}
            onChange={e => setSearchTerm(e.target.value)}
          />
        </div>

        <div className="mt-4 flex items-center md:mt-0 md:ml-4">
          <label htmlFor="status" className="block text-sm font-medium text-gray-700 mr-2">
            Status:
          </label>
          <select
            id="status"
            className="block w-full rounded-md border-0 py-2 pl-3 pr-10 text-gray-900 ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-indigo-600 sm:text-sm"
            value={statusFilter}
            onChange={e => setStatusFilter(e.target.value as any)}
          >
            <option value="svi">Svi</option>
            <option value="aktivan">Aktivni</option>
            <option value="isplaćen">Isplaćeni</option>
            <option value="istekao">Istekli</option>
          </select>
        </div>
      </div>

      <div className="mt-8">
        <div className="overflow-x-auto shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
          {loading && pledges.length > 0 && (
            <div className="bg-blue-50 px-4 py-2 text-sm text-blue-700">
              Obrada...
            </div>
          )}

          <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Broj</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Datum</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Klijent</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Predmet</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Procjena</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Pozajmica</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Za isplatu</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Rok</th>
                <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Status</th>
                <th className="px-3 py-3.5 text-right text-sm font-semibold text-gray-900">Akcije</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              {filteredPledges.map(p => (
                <tr key={p.id} className="hover:bg-gray-50">
                  <td className="px-3 py-2 text-sm font-medium text-gray-900">{p.id}</td>
                  <td className="px-3 py-2 text-sm text-gray-500 whitespace-nowrap">
                    {new Date(p.pledgeDate).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-3 py-2 text-sm text-gray-900">{p.clientName}</td>
                  <td className="px-3 py-2 text-sm text-gray-500 max-w-xs truncate" title={p.itemName}>
                    {p.itemName}
                  </td>
                  <td className="px-3 py-2 text-sm text-gray-500 whitespace-nowrap">
                    {p.estimatedValue.toFixed(2)} €
                  </td>
                  <td className="px-3 py-2 text-sm font-medium text-gray-900 whitespace-nowrap">
                    {p.loanAmount.toFixed(2)} €
                  </td>
                  <td className="px-3 py-2 text-sm font-medium text-indigo-600 whitespace-nowrap">
                    {p.returnAmount.toFixed(2)} €
                  </td>
                  <td className="px-3 py-2 text-sm text-gray-500 whitespace-nowrap">
                    {new Date(p.redeemDeadline).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-3 py-2 text-sm">
                    {p.redeemed ? (
                      <span className="inline-flex rounded-full bg-green-100 px-2 py-1 text-xs font-semibold text-green-800">
                        Isplaćen
                      </span>
                    ) : p.forfeited ? (
                      <span className="inline-flex rounded-full bg-purple-100 px-2 py-1 text-xs font-semibold text-purple-800">
                        U prodaji
                      </span>
                    ) : new Date(p.redeemDeadline) < new Date() ? (
                      <span className="inline-flex rounded-full bg-red-100 px-2 py-1 text-xs font-semibold text-red-800">
                        Istekao
                      </span>
                    ) : (
                      <span className="inline-flex rounded-full bg-blue-100 px-2 py-1 text-xs font-semibold text-blue-800">
                        Aktivan
                      </span>
                    )}
                  </td>
                  <td className="px-3 py-2 text-right whitespace-nowrap">
                    <div className="flex justify-end gap-2">
                      {!p.redeemed && !p.forfeited && (
                        <>
                          <button
                            onClick={() => handleRedeem(p.id)}
                            className="text-green-600 hover:text-green-900"
                            title="Isplaćeno - klijent preuzeo predmet"
                          >
                            <CheckCircleIcon className="h-5 w-5" />
                          </button>
                          <button
                            onClick={() => handleForfeit(p.id)}
                            className="text-purple-600 hover:text-purple-900"
                            title="Prenesi u prodaju - preuzeto u vlasništvo"
                          >
                            <ShoppingCartIcon className="h-5 w-5" />
                          </button>
                        </>
                      )}
                      <button
                        onClick={() => handleGeneratePledgeAgreement(p.id)}
                        className="text-blue-600 hover:text-blue-900"
                        title="Generiraj ugovor o zalogu"
                      >
                        <DocumentTextIcon className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() => handleEdit(p.id)}
                        className="text-indigo-600 hover:text-indigo-900"
                        title="Uredi"
                      >
                        <PencilIcon className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() => handleDelete(p.id)}
                        className="text-red-600 hover:text-red-800"
                        title="Obriši"
                      >
                        <TrashIcon className="h-5 w-5" />
                      </button>
                      <button
                        className="text-gray-500 hover:text-gray-700"
                        title="Pregled"
                      >
                        <EyeIcon className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {filteredPledges.length === 0 && (
                <tr>
                  <td colSpan={10} className="px-3 py-8 text-center text-gray-500">
                    Nema pronađenih zaloga.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default PledgeListPage;
