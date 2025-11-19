import { Vacation, VacationType } from '../types/HR';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

export interface VacationRequestData {
    startDate: string;
    endDate: string;
    type: VacationType;
    reason?: string;
}

export interface VacationApprovalData {
    vacationId: string;
    rejectionReason?: string;
}

// Submit a vacation request (sends email to admin)
export const submitVacationRequest = async (data: VacationRequestData): Promise<Vacation> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation/request`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Failed to submit vacation request');
        }

        return await response.json();
    } catch (error) {
        console.error('Error submitting vacation request:', error);
        throw error;
    }
};

// Get all vacation requests (admin only)
export const getAllVacations = async (): Promise<Vacation[]> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch vacations');
        }

        return await response.json();
    } catch (error) {
        console.error('Error fetching vacations:', error);
        throw error;
    }
};

// Approve a vacation request
export const approveVacation = async (vacationId: string): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation/${vacationId}/approve`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to approve vacation');
        }
    } catch (error) {
        console.error('Error approving vacation:', error);
        throw error;
    }
};

// Reject a vacation request
export const rejectVacation = async (vacationId: string, rejectionReason?: string): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation/${vacationId}/reject`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ rejectionReason }),
        });

        if (!response.ok) {
            throw new Error('Failed to reject vacation');
        }
    } catch (error) {
        console.error('Error rejecting vacation:', error);
        throw error;
    }
};

// Delete a vacation request
export const deleteVacation = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to delete vacation');
        }
    } catch (error) {
        console.error('Error deleting vacation:', error);
        throw error;
    }
};

// Update a vacation request
export const updateVacation = async (id: string, vacation: Vacation): Promise<void> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Vacation/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(vacation),
        });

        if (!response.ok) {
            throw new Error('Failed to update vacation');
        }
    } catch (error) {
        console.error('Error updating vacation:', error);
        throw error;
    }
};
