import React, { useState, useEffect } from 'react';
import { PayrollModal } from '../components/payroll/PayrollModal';
import { PayrollTable } from '../components/payroll/PayrollTable';
import { Employee, Payroll, Loan } from '../types/HR';
import LoanModal from '../components/loans/LoanModal';
import LoansTable from '../components/loans/LoansTable';
import { payrollApi } from '../services/payrollApi';
import { employeeApi } from '../services/employeeApi';

const PayrollPage: React.FC = () => {
  // Data state
  const [payrolls, setPayrolls] = useState<Payroll[]>([]);
  const [loans, setLoans] = useState<Loan[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'payroll' | 'loans'>('payroll');
  
  // Stanja za modale
  const [isPayrollModalOpen, setIsPayrollModalOpen] = useState(false);
  const [editingPayroll, setEditingPayroll] = useState<Payroll | null>(null);
  const [isLoanModalOpen, setIsLoanModalOpen] = useState(false);
  const [editingLoan, setEditingLoan] = useState<Loan | null>(null);

  // Load data from backend
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [payrollsData, employeesData] = await Promise.all([
        payrollApi.getAll(),
        employeeApi.getAllEmployees()
      ]);

      setPayrolls(payrollsData);
      setEmployees(employeesData);
      // Note: Employee loans API not implemented yet
      setLoans([]);
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

  // Funkcije za plaće
  const handleSavePayroll = async (payroll: Payroll) => {
    try {
      setLoading(true);
      if (editingPayroll) {
        await payrollApi.update(editingPayroll.id, payroll);
        setPayrolls(payrolls.map(p => p.id === payroll.id ? payroll : p));
        alert('Plaća uspješno ažurirana!');
      } else {
        const newPayroll = await payrollApi.create(payroll);
        setPayrolls([...payrolls, newPayroll]);
        alert('Plaća uspješno dodana!');
      }
      setIsPayrollModalOpen(false);
      setEditingPayroll(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to save payroll';
      alert('Greška pri spremanju: ' + message);
      console.error('Error saving payroll:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleEditPayroll = (payroll: Payroll) => {
    setEditingPayroll(payroll);
    setIsPayrollModalOpen(true);
  };

  const handleAddNewPayroll = () => {
    setEditingPayroll(null);
    setIsPayrollModalOpen(true);
  };

  const handleDeletePayroll = async (id: string) => {
    if (!window.confirm('Jeste li sigurni da želite obrisati ovaj obračun?')) return;

    try {
      setLoading(true);
      await payrollApi.delete(id);
      setPayrolls(payrolls.filter(p => p.id !== id));
      alert('Obračun uspješno obrisan!');
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to delete payroll';
      alert('Greška pri brisanju: ' + message);
      console.error('Error deleting payroll:', err);
    } finally {
      setLoading(false);
    }
  }

  // Funkcije za pozajmice
  const handleSaveLoan = (loan: Loan) => {
    if (editingLoan) {
        setLoans(loans.map(l => l.id === loan.id ? loan : l));
    } else {
        setLoans([...loans, loan]);
    }
    setIsLoanModalOpen(false);
  };
  const handleEditLoan = (loan: Loan) => {
    setEditingLoan(loan);
    setIsLoanModalOpen(true);
  };
  const handleAddNewLoan = () => {
    setEditingLoan(null);
    setIsLoanModalOpen(true);
  };
  const handleDeleteLoan = (id: string) => {
      setLoans(loans.filter(l => l.id !== id));
  }

  if (loading && payrolls.length === 0) {
    return (
      <div className="p-6 flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Učitavanje podataka...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      {error && (
        <div className="mb-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
          <p className="text-yellow-800">Upozorenje: {error}</p>
        </div>
      )}

      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8" aria-label="Tabs">
          <button onClick={() => setActiveTab('payroll')} className={`${activeTab === 'payroll' ? 'border-indigo-500 text-indigo-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'} whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm`}>
            Evidencija plaća
          </button>
          <button onClick={() => setActiveTab('loans')} className={`${activeTab === 'loans' ? 'border-indigo-500 text-indigo-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'} whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm`}>
            Evidencija pozajmica
          </button>
        </nav>
      </div>
      <div className="mt-6">
        {activeTab === 'payroll' && (
          <div>
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-semibold">Obračuni plaća</h2>
                <button onClick={handleAddNewPayroll} className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded-md shadow-sm">Novi obračun</button>
            </div>
            <PayrollTable data={payrolls} onEdit={handleEditPayroll} onDelete={handleDeletePayroll} />
          </div>
        )}
        {activeTab === 'loans' && (
          <div>
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-semibold">Pozajmice</h2>
                <button onClick={handleAddNewLoan} className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded-md shadow-sm">Nova pozajmica</button>
            </div>
            <LoansTable data={loans} onEdit={handleEditLoan} onDelete={handleDeleteLoan} />
          </div>
        )}
      </div>

        <PayrollModal isOpen={isPayrollModalOpen} onClose={() => setIsPayrollModalOpen(false)} onSave={handleSavePayroll} initialData={editingPayroll} employees={employees} />
        <LoanModal isOpen={isLoanModalOpen} onClose={() => setIsLoanModalOpen(false)} onSave={handleSaveLoan} initialData={editingLoan} employees={employees} />
    </div>
  );
};

export default PayrollPage;

