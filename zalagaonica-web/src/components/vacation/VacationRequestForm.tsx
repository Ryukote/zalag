import React, { useState } from 'react';
import { CalendarIcon } from '@heroicons/react/24/outline';
import { submitVacationRequest } from '../../services/vacationApi';
import { VacationType } from '../../types/HR';

interface Props {
  onSuccess?: () => void;
}

export const VacationRequestForm: React.FC<Props> = ({ onSuccess }) => {
  const [formData, setFormData] = useState({
    startDate: new Date().toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0],
    type: VacationType.AnnualLeave,
    reason: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    if (name === 'type') {
      setFormData(prev => ({ ...prev, [name]: parseInt(value) as VacationType }));
    } else {
      setFormData(prev => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Validate dates
    if (new Date(formData.endDate) < new Date(formData.startDate)) {
      setErrorMessage('Datum završetka mora biti poslije datuma početka');
      return;
    }

    setIsSubmitting(true);
    setErrorMessage('');
    setSuccessMessage('');

    try {
      await submitVacationRequest({
        startDate: formData.startDate,
        endDate: formData.endDate,
        type: formData.type,
        reason: formData.reason
      });

      setSuccessMessage('Zahtjev za godišnji odmor je uspješno poslan!');

      // Reset form
      setFormData({
        startDate: new Date().toISOString().split('T')[0],
        endDate: new Date().toISOString().split('T')[0],
        type: VacationType.AnnualLeave,
        reason: ''
      });

      if (onSuccess) {
        onSuccess();
      }
    } catch (error) {
      setErrorMessage('Greška prilikom slanja zahtjeva. Molimo pokušajte ponovno.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bg-white shadow-md rounded-lg p-6 max-w-2xl mx-auto">
      <div className="flex items-center mb-6">
        <CalendarIcon className="h-8 w-8 text-indigo-600 mr-3" />
        <h2 className="text-2xl font-bold text-gray-900">Zahtjev za Godišnji Odmor</h2>
      </div>

      {successMessage && (
        <div className="mb-4 p-4 bg-green-100 text-green-800 rounded-md">
          {successMessage}
        </div>
      )}

      {errorMessage && (
        <div className="mb-4 p-4 bg-red-100 text-red-800 rounded-md">
          {errorMessage}
        </div>
      )}

      <form onSubmit={handleSubmit}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <div>
            <label htmlFor="startDate" className="block text-sm font-medium text-gray-700 mb-2">
              Datum od
            </label>
            <input
              type="date"
              id="startDate"
              name="startDate"
              value={formData.startDate}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
            />
          </div>

          <div>
            <label htmlFor="endDate" className="block text-sm font-medium text-gray-700 mb-2">
              Datum do
            </label>
            <input
              type="date"
              id="endDate"
              name="endDate"
              value={formData.endDate}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
            />
          </div>
        </div>

        <div className="mb-4">
          <label htmlFor="type" className="block text-sm font-medium text-gray-700 mb-2">
            Tip odsutnosti
          </label>
          <select
            id="type"
            name="type"
            value={formData.type}
            onChange={handleChange}
            required
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
          >
            <option value={VacationType.AnnualLeave}>Godišnji odmor</option>
            <option value={VacationType.SickLeave}>Bolovanje</option>
            <option value={VacationType.PaidLeave}>Plaćeni dopust</option>
          </select>
        </div>

        <div className="mb-6">
          <label htmlFor="reason" className="block text-sm font-medium text-gray-700 mb-2">
            Razlog (opcionalno)
          </label>
          <textarea
            id="reason"
            name="reason"
            value={formData.reason}
            onChange={handleChange}
            rows={3}
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
            placeholder="Unesite razlog ako je potrebno..."
          />
        </div>

        <div className="flex justify-end">
          <button
            type="submit"
            disabled={isSubmitting}
            className="px-6 py-3 bg-indigo-600 text-white font-semibold rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isSubmitting ? 'Šaljem...' : 'Pošalji'}
          </button>
        </div>
      </form>
    </div>
  );
};
