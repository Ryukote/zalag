import React, { useState, useEffect, useMemo } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import { Pagination } from '../components/ui/Pagination';
import { PlusIcon } from '@heroicons/react/24/outline';
import { ConfirmDialog } from '../components/ui/ConfirmDialog';
import { UnitOfMeasureModal } from '../components/units-of-measure/UnitOfMeasureModal';
import { UnitOfMeasureTable } from '../components/units-of-measure/UnitOfMeasureTable';
import { UnitOfMeasure } from '../types/unitsOfMeasure';
import { unitOfMeasureApi } from '../services/unitOfMeasureApi';

const ITEMS_PER_PAGE_OPTIONS = [5, 10, 20];

const UnitOfMeasurePage: React.FC = () => {
    const [units, setUnits] = useState<UnitOfMeasure[]>([]);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [editingUnit, setEditingUnit] = useState<UnitOfMeasure | null>(null);
    const [unitToDelete, setUnitToDelete] = useState<string | null>(null);

    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(ITEMS_PER_PAGE_OPTIONS[0]);

    useEffect(() => {
        loadUnits();
    }, []);

    const loadUnits = async () => {
        try {
            setLoading(true);
            const data = await unitOfMeasureApi.getAll();
            setUnits(data);
        } catch (error) {
            console.error('Failed to load units of measure:', error);
            alert('Failed to load units of measure');
        } finally {
            setLoading(false);
        }
    };

    // Logika za paginaciju
    const paginatedUnits = useMemo(() => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        return units.slice(startIndex, endIndex);
    }, [units, currentPage, itemsPerPage]);

    const handleOpenAddModal = () => {
        setEditingUnit(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (item: UnitOfMeasure) => {
        setEditingUnit(item);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingUnit(null);
    };

    const handleSaveUnit = async (newUnit: UnitOfMeasure) => {
        try {
            if (editingUnit) {
                await unitOfMeasureApi.update(newUnit.id, newUnit);
            } else {
                await unitOfMeasureApi.create(newUnit);
            }
            await loadUnits();
            handleCloseModal();
        } catch (error) {
            console.error('Failed to save unit of measure:', error);
            alert('Failed to save unit of measure');
        }
    };

    const handleConfirmDelete = (id: string) => {
        setUnitToDelete(id);
        setIsConfirmOpen(true);
    };

    const handleDeleteUnit = async () => {
        if (!unitToDelete) return;

        try {
            await unitOfMeasureApi.delete(unitToDelete);
            await loadUnits();
            setIsConfirmOpen(false);
            setUnitToDelete(null);
        } catch (error) {
            console.error('Failed to delete unit of measure:', error);
            alert('Failed to delete unit of measure');
        }
    };

    return (
        <AppLayout>
            <Header title="Jedinice Mjere" showBackButton={true} />
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 flex-grow">
                <div className="sm:flex sm:items-center">
                    <div className="sm:flex-auto">
                        <h1 className="text-xl font-semibold text-gray-900">Jedinice Mjere</h1>
                        <p className="mt-2 text-sm text-gray-700">Popis svih jedinica mjere.</p>
                    </div>
                    <div className="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
                        <button
                            type="button"
                            onClick={handleOpenAddModal}
                            className="inline-flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
                        >
                            <PlusIcon className="h-5 w-5 mr-2" />
                            Nova jedinica mjere
                        </button>
                    </div>
                </div>

                <div className="mt-8 flow-root">
                    <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                        <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                            {loading ? (
                                <div className="text-center py-4">Loading...</div>
                            ) : (
                                <>
                                    <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                                        <UnitOfMeasureTable
                                            data={paginatedUnits}
                                            onEdit={handleOpenEditModal}
                                            onDelete={handleConfirmDelete}
                                        />
                                    </div>
                                    <Pagination
                                        currentPage={currentPage}
                                        totalCount={units.length}
                                        itemsPerPage={itemsPerPage}
                                        onPageChange={page => setCurrentPage(page)}
                                        onItemsPerPageChange={setItemsPerPage}
                                    />
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </div>

            <UnitOfMeasureModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                onSave={handleSaveUnit}
                initialData={editingUnit}
            />
            <ConfirmDialog
                isOpen={isConfirmOpen}
                onClose={() => setIsConfirmOpen(false)}
                onConfirm={handleDeleteUnit}
                title="Potvrda brisanja"
                message="Jeste li sigurni da želite obrisati ovu jedinicu mjere? Ova radnja se ne može poništiti."
            />
        </AppLayout>
    );
};

export default UnitOfMeasurePage;
