import { Employee } from '../types/HR';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

export const getAllEmployees = async (): Promise<Employee[]> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Employee`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch employees');
        }

        return await response.json();
    } catch (error) {
        console.error('Error fetching employees:', error);
        throw error;
    }
};

export const getEmployeeById = async (id: string): Promise<Employee> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Employee/${id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch employee');
        }

        return await response.json();
    } catch (error) {
        console.error('Error fetching employee:', error);
        throw error;
    }
};

export const createEmployee = async (employee: Omit<Employee, 'id'>): Promise<Employee> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Employee`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(employee),
        });

        if (!response.ok) {
            throw new Error('Failed to create employee');
        }

        return await response.json();
    } catch (error) {
        console.error('Error creating employee:', error);
        throw error;
    }
};

export const updateEmployee = async (id: string, employee: Employee): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Employee/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(employee),
        });

        if (!response.ok) {
            throw new Error('Failed to update employee');
        }
    } catch (error) {
        console.error('Error updating employee:', error);
        throw error;
    }
};

export const deleteEmployee = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Employee/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to delete employee');
        }
    } catch (error) {
        console.error('Error deleting employee:', error);
        throw error;
    }
};

export const employeeApi = {
    getAllEmployees,
    getEmployeeById,
    createEmployee,
    updateEmployee,
    deleteEmployee,
};

export default employeeApi;
