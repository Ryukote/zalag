import React, { useState, useEffect } from 'react';
import { DeliveryCostModal } from '../components/delivery-cost/DeliveryCostModal';
import { DeliveryCostsTable } from '../components/delivery-cost/DeliveryCostsTable';
import { DeliveryCost } from '../types/DeliveryCost';
import * as deliveryCostApi from '../services/deliveryCostApi';

const DeliveryCostsPage: React.FC = () => {
  const [costs, setCosts] = useState<DeliveryCost[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCost, setEditingCost] = useState<DeliveryCost | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadCosts();
  }, []);

  const loadCosts = async () => {
    try {
      setLoading(true);
      const data = await deliveryCostApi.getAll();
      setCosts(data);
    } catch (error) {
      console.error('Failed to load delivery costs:', error);
      alert('Failed to load delivery costs');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (cost: DeliveryCost) => {
      setEditingCost(cost);
      setIsModalOpen(true);
  }

  const handleSave = async (cost: DeliveryCost) => {
      try {
        if (editingCost) {
          await deliveryCostApi.update(cost);
        } else {
          await deliveryCostApi.create(cost);
        }
        await loadCosts();
        setIsModalOpen(false);
      } catch (error) {
        console.error('Failed to save delivery cost:', error);
        alert('Failed to save delivery cost');
      }
  }

  const handleDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this delivery cost?')) return;

    try {
      await deliveryCostApi.remove(id);
      await loadCosts();
    } catch (error) {
      console.error('Failed to delete delivery cost:', error);
      alert('Failed to delete delivery cost');
    }
  }

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-semibold">Troškovi dostave paketa</h1>
        <button onClick={() => { setEditingCost(null); setIsModalOpen(true); }} className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded-md shadow-sm">
          Dodaj novi trošak
        </button>
      </div>

      {loading ? (
        <div className="text-center py-4">Loading...</div>
      ) : (
        <DeliveryCostsTable data={costs} onEdit={handleEdit} onDelete={handleDelete}/>
      )}

      {isModalOpen && (
        <DeliveryCostModal 
            isOpen={isModalOpen}
            initialData={editingCost}
            onClose={() => setIsModalOpen(false)}
            onSave={handleSave}
        />
      )}
    </div>
  );
};

export default DeliveryCostsPage;
