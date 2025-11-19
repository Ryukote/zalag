import React, { useEffect, useState } from "react";
import LoansTable from "../components/loans/LoansTable";
import LoanModal from "../components/loans/LoanModal";
import { loanApi, Loan } from "../services/loanApi";

const LoanPage: React.FC = () => {
  const [loans, setLoans] = useState<Loan[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedLoan, setSelectedLoan] = useState<Loan | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchLoans = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await loanApi.getAll();
      setLoans(data);
    } catch (e) {
      const message = e instanceof Error ? e.message : 'Failed to load loans';
      setError(message);
      console.error("Error fetching loans", e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchLoans();
  }, []);

  const onEdit = (loan: any) => {
    setSelectedLoan(loan);
    setModalOpen(true);
  };

  const onDelete = async (id: any) => {
    if (!window.confirm("Jeste li sigurni da želite obrisati ovaj zajam?")) return;

    try {
      setLoading(true);
      await loanApi.delete(id);
      setLoans(prev => prev.filter((l: any) => l.id !== id));
      alert('Zajam uspješno obrisan!');
    } catch (e) {
      const message = e instanceof Error ? e.message : 'Failed to delete loan';
      alert('Greška pri brisanju: ' + message);
      console.error("Error deleting loan", e);
    } finally {
      setLoading(false);
    }
  };

  const onSave = async (loan: any) => {
    try {
      setLoading(true);
      if (loan.id) {
        await loanApi.update(loan.id, loan);
        setLoans(prev => prev.map((l: any) => l.id === loan.id ? loan : l));
        alert('Zajam uspješno ažuriran!');
      } else {
        const newLoan = await loanApi.create(loan);
        setLoans(prev => [...prev, newLoan]);
        alert('Zajam uspješno dodan!');
      }
      setModalOpen(false);
      setSelectedLoan(null);
    } catch (e) {
      const message = e instanceof Error ? e.message : 'Failed to save loan';
      alert('Greška pri spremanju: ' + message);
      console.error("Error saving loan", e);
    } finally {
      setLoading(false);
    }
  };

  const onClose = () => {
    setModalOpen(false);
    setSelectedLoan(null);
  };

  return (
    <div className="p-4">
      <h1 className="font-semibold text-xl mb-4">Zajmovi</h1>

      {error && (
        <div className="mb-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
          <p className="text-yellow-800">Upozorenje: {error}</p>
        </div>
      )}

      <button
        className="mb-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        onClick={() => setModalOpen(true)}
      >
        Add Loan
      </button>

      <LoansTable loans={loans} loading={loading} onEdit={onEdit} onDelete={onDelete} />

      {modalOpen && (
        <LoanModal loan={selectedLoan} onSave={onSave} onClose={onClose} />
      )}
    </div>
  );
};

export default LoanPage;
