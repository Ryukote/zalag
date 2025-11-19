import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { AdminVacationsTable } from '../components/vacation/AdminVacationsTable';
import { VacationModal } from '../components/vacation/VacationModal';
import { Vacation, Employee } from '../types/HR';
import { getAllVacations, approveVacation, rejectVacation, deleteVacation, updateVacation } from '../services/vacationApi';
import { getAllEmployees } from '../services/employeeApi';
import { PlusIcon } from '@heroicons/react/24/outline';

export const AdminVacationPage: React.FC = () => {
  const [vacations, setVacations] = useState<Vacation[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedVacation, setSelectedVacation] = useState<Vacation | null>(null);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setIsLoading(true);
    setError('');
    try {
      const [vacationsData, employeesData] = await Promise.all([
        getAllVacations(),
        getAllEmployees()
      ]);
      setVacations(vacationsData);
      setEmployees(employeesData);
    } catch (err) {
      setError('Greška prilikom učitavanja podataka');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  const loadVacations = async () => {
    try {
      const data = await getAllVacations();
      setVacations(data);
    } catch (err) {
      setError('Greška prilikom učitavanja zahtjeva');
      console.error(err);
    }
  };

  const handleEdit = (vacation: Vacation) => {
    setSelectedVacation(vacation);
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteVacation(id);
      await loadVacations();
    } catch (err) {
      setError('Greška prilikom brisanja zahtjeva');
      console.error(err);
    }
  };

  const handleApprove = async (id: string) => {
    try {
      await approveVacation(id);
      await loadVacations();
    } catch (err) {
      setError('Greška prilikom odobravanja zahtjeva');
      console.error(err);
    }
  };

  const handleReject = async (id: string) => {
    try {
      await rejectVacation(id);
      await loadVacations();
    } catch (err) {
      setError('Greška prilikom odbijanja zahtjeva');
      console.error(err);
    }
  };

  const handleSave = async (vacation: Vacation) => {
    try {
      await updateVacation(vacation.id, vacation);
      setIsModalOpen(false);
      await loadVacations();
    } catch (err) {
      setError('Greška prilikom spremanja zahtjeva');
      console.error(err);
    }
  };

  const handleAddNew = () => {
    setSelectedVacation(null);
    setIsModalOpen(true);
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Upravljanje Godišnjim Odmorima</h1>
            <p className="mt-2 text-sm text-gray-600">
              Pregledajte i upravljajte zahtjevima zaposlenika za godišnji odmor
            </p>
          </div>
          <button
            onClick={handleAddNew}
            className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
          >
            <PlusIcon className="h-5 w-5 mr-2" />
            Dodaj novi zahtjev
          </button>
        </div>

        {error && (
          <div className="mb-4 p-4 bg-red-100 text-red-800 rounded-md">
            {error}
          </div>
        )}

        {isLoading ? (
          <div className="flex justify-center items-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
          </div>
        ) : (
          <AdminVacationsTable
            data={vacations}
            onEdit={handleEdit}
            onDelete={handleDelete}
            onApprove={handleApprove}
            onReject={handleReject}
          />
        )}

        <VacationModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSave}
          initialData={selectedVacation}
          employees={employees}
        />
      </div>
    </AppLayout>
  );
};
