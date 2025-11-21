import api from './api';

export interface UserAccount {
  id: string;
  username: string;
  email: string;
  isActive: boolean;
  createdAt: string;
  roles: string[];
}

export interface CreateUserDto {
  username: string;
  email: string;
  password: string;
  roleIds?: string[];
}

export interface UpdateUserDto {
  username?: string;
  email?: string;
  isActive?: boolean;
  newPassword?: string;
}

export const userManagementApi = {
  getAllUsers: async (): Promise<UserAccount[]> => {
    const response = await api.get('/UserManagement');
    return response.data;
  },

  getUserById: async (id: string): Promise<UserAccount> => {
    const response = await api.get(`/UserManagement/${id}`);
    return response.data;
  },

  createUser: async (user: CreateUserDto): Promise<UserAccount> => {
    const response = await api.post('/UserManagement', user);
    return response.data;
  },

  updateUser: async (id: string, user: UpdateUserDto): Promise<void> => {
    await api.put(`/UserManagement/${id}`, user);
  },

  assignRoles: async (id: string, roleIds: string[]): Promise<void> => {
    await api.post(`/UserManagement/${id}/roles`, roleIds);
  },

  deleteUser: async (id: string): Promise<void> => {
    await api.delete(`/UserManagement/${id}`);
  }
};
