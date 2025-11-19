import React from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { VacationRequestForm } from '../components/vacation/VacationRequestForm';

export const WorkerVacationPage: React.FC = () => {
  const handleSuccess = () => {
    // Optional: Show additional confirmation or refresh data
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Zahtjev za Godišnji Odmor</h1>
          <p className="mt-2 text-sm text-gray-600">
            Popunite obrazac ispod kako biste poslali zahtjev za godišnji odmor. Administrator će biti obaviješten putem e-maila.
          </p>
        </div>

        <VacationRequestForm onSuccess={handleSuccess} />
      </div>
    </AppLayout>
  );
};
