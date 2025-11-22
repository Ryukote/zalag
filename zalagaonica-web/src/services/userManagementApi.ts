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
    return await api.get('/UserManagement');
  },

  getUserById: async (id: string): Promise<UserAccount> => {
    return await api.get(`/UserManagement/${id}`);
  },

  createUser: async (user: CreateUserDto): Promise<UserAccount> => {
    return await api.post('/UserManagement', user);
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
