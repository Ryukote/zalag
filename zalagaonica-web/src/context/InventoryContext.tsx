import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { articleApi, Article as ApiArticle } from '../services/articleApi';
import { clientApi, Client as ApiClient } from '../services/clientApi';
import { pledgeApi, Pledge as ApiPledge } from '../services/pledgeApi';

// ----------------------
// INTERFACE DEFINICIJE
// ----------------------
export interface Article extends ApiArticle {}

export interface Client extends ApiClient {}

export interface Pledge {
  id: string;
  clientId: string;
  clientName: string;
  itemName: string;
  itemDescription: string;
  estimatedValue: number;
  loanAmount: number;
  returnAmount: number;
  period: number;
  pledgeDate: Date;
  redeemDeadline: Date;
  redeemed: boolean;
  forfeited: boolean;
  itemImages: string[];
  warrantyFiles: string[];
}

export interface SaleData {
  articleId: string;
  quantity: number;
  salePrice: number;
  date: Date;
}

export interface PurchaseRecord {
  id: string;
  clientId: string;
  clientName: string;
  itemName: string;
  itemDescription: string;
  estimatedValue: number;
  purchaseAmount: number;
  totalAmount: number;
  purchaseDate: Date;
  paymentDate: string;
  itemImages: string[];
  warrantyFiles: string[];
}

// ----------------------
// KONTEKST
// ----------------------
interface InventoryContextType {
  articles: Article[];
  pledges: Pledge[];
  saleRecords: SaleData[];
  clients: Client[];
  purchases: PurchaseRecord[];
  loading: boolean;
  error: string | null;

  // Article operations
  addArticle: (article: Omit<Article, 'id' | 'createdAt' | 'updatedAt'>) => Promise<void>;
  updateArticle: (article: Article) => Promise<void>;
  removeArticle: (id: string) => Promise<void>;

  // Client operations
  addClient: (client: Omit<Client, 'id' | 'createdAt' | 'updatedAt'>) => Promise<void>;
  updateClient: (client: Client) => Promise<void>;
  deleteClient: (id: string) => Promise<void>;

  // Pledge operations
  createPledge: (pledge: Omit<Pledge, 'id' | 'createdAt' | 'updatedAt'>) => Promise<void>;
  updatePledge: (pledge: Pledge) => Promise<void>;
  redeemPledge: (id: string) => Promise<void>;
  forfeitPledge: (id: string) => Promise<void>;
  cancelPledge: (id: string) => Promise<void>;
  deletePledge: (id: string) => Promise<void>;

  // Other operations
  sale: (saleData: SaleData) => Promise<boolean>;
  createPurchase: (purchase: PurchaseRecord) => Promise<boolean>;
  getClients: () => Client[];
  refreshData: () => Promise<void>;
}

const InventoryContext = createContext<InventoryContextType | undefined>(undefined);

export const InventoryProvider = ({ children }: { children: ReactNode }) => {
  const [articles, setArticles] = useState<Article[]>([]);
  const [clients, setClients] = useState<Client[]>([]);
  const [pledges, setPledges] = useState<Pledge[]>([]);
  const [sales, setSales] = useState<SaleData[]>([]);
  const [purchases, setPurchases] = useState<PurchaseRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Helper function to convert API pledge to local pledge format
  const convertPledge = (apiPledge: ApiPledge): Pledge => ({
    ...apiPledge,
    pledgeDate: new Date(apiPledge.pledgeDate),
    redeemDeadline: new Date(apiPledge.redeemDeadline),
    itemImages: apiPledge.itemImagesJson ? JSON.parse(apiPledge.itemImagesJson) : [],
    warrantyFiles: apiPledge.warrantyFilesJson ? JSON.parse(apiPledge.warrantyFilesJson) : [],
  });

  // Load all data from API on mount
  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [articlesData, clientsData, pledgesData] = await Promise.all([
        articleApi.getAll().catch(() => []),
        clientApi.getAll().catch(() => []),
        pledgeApi.getAll().catch(() => []),
      ]);

      setArticles(articlesData);
      setClients(clientsData);
      setPledges(pledgesData.map(convertPledge));
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to load data';
      setError(message);
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  // ----------------------
  // ARTICLES
  // ----------------------
  const addArticle = async (article: Omit<Article, 'id' | 'createdAt' | 'updatedAt'>) => {
    try {
      const newArticle = await articleApi.create(article);
      setArticles(prev => [...prev, newArticle]);
    } catch (err) {
      console.error('Error adding article:', err);
      throw err;
    }
  };

  const updateArticle = async (article: Article) => {
    try {
      await articleApi.update(article.id, article);
      setArticles(prev => prev.map(a => (a.id === article.id ? article : a)));
    } catch (err) {
      console.error('Error updating article:', err);
      throw err;
    }
  };

  const removeArticle = async (id: string) => {
    try {
      await articleApi.delete(id);
      setArticles(prev => prev.filter(a => a.id !== id));
    } catch (err) {
      console.error('Error removing article:', err);
      throw err;
    }
  };

  // ----------------------
  // CLIENTS
  // ----------------------
  const addClient = async (client: Omit<Client, 'id' | 'createdAt' | 'updatedAt'>) => {
    try {
      const newClient = await clientApi.create(client);
      setClients(prev => [...prev, newClient]);
    } catch (err) {
      console.error('Error adding client:', err);
      throw err;
    }
  };

  const updateClient = async (client: Client) => {
    try {
      await clientApi.update(client.id, client);
      setClients(prev => prev.map(c => (c.id === client.id ? client : c)));
    } catch (err) {
      console.error('Error updating client:', err);
      throw err;
    }
  };

  const deleteClient = async (id: string) => {
    try {
      await clientApi.delete(id);
      setClients(prev => prev.filter(c => c.id !== id));
    } catch (err) {
      console.error('Error deleting client:', err);
      throw err;
    }
  };

  const getClients = () => clients;

  // ----------------------
  // PLEDGES
  // ----------------------
  const createPledge = async (pledge: Omit<Pledge, 'id' | 'createdAt' | 'updatedAt'>) => {
    try {
      const pledgeDto = {
        ...pledge,
        pledgeDate: pledge.pledgeDate.toISOString(),
        redeemDeadline: pledge.redeemDeadline.toISOString(),
        itemImagesJson: JSON.stringify(pledge.itemImages || []),
        warrantyFilesJson: JSON.stringify(pledge.warrantyFiles || []),
      };

      const newPledge = await pledgeApi.create(pledgeDto);
      setPledges(prev => [...prev, convertPledge(newPledge)]);
    } catch (err) {
      console.error('Error creating pledge:', err);
      throw err;
    }
  };

  const updatePledge = async (pledge: Pledge) => {
    try {
      const pledgeDto = {
        id: pledge.id,
        clientId: pledge.clientId,
        clientName: pledge.clientName,
        itemName: pledge.itemName,
        itemDescription: pledge.itemDescription,
        estimatedValue: pledge.estimatedValue,
        loanAmount: pledge.loanAmount,
        returnAmount: pledge.returnAmount,
        period: pledge.period,
        pledgeDate: pledge.pledgeDate.toISOString(),
        redeemDeadline: pledge.redeemDeadline.toISOString(),
        itemImagesJson: JSON.stringify(pledge.itemImages || []),
        warrantyFilesJson: JSON.stringify(pledge.warrantyFiles || []),
      };

      await pledgeApi.update(pledge.id, pledgeDto);
      setPledges(prev => prev.map(p => (p.id === pledge.id ? pledge : p)));
    } catch (err) {
      console.error('Error updating pledge:', err);
      throw err;
    }
  };

  const redeemPledge = async (id: string) => {
    try {
      await pledgeApi.redeem(id);
      setPledges(prev => prev.map(p => (p.id === id ? { ...p, redeemed: true } : p)));
    } catch (err) {
      console.error('Error redeeming pledge:', err);
      throw err;
    }
  };

  const forfeitPledge = async (id: string) => {
    try {
      await pledgeApi.forfeit(id);

      // Get the pledge to create article from it
      const pledge = pledges.find(p => p.id === id);
      if (pledge) {
        // Create article in main warehouse from pledged item
        const newArticle = {
          name: pledge.itemName,
          description: `Zalog - ${pledge.itemDescription}`,
          purchasePrice: pledge.loanAmount,
          retailPrice: pledge.estimatedValue,
          taxRate: 25,
          stock: 1,
          unitOfMeasureCode: 'KOM',
          supplierName: pledge.clientName,
          group: 'Zalog - Preuzeto u vlasništvo',
          status: 'available',
          warehouseType: 'main',
        };

        await articleApi.create(newArticle);
        await loadData(); // Reload all data to get the new article
      }

      setPledges(prev => prev.map(p => (p.id === id ? { ...p, forfeited: true } : p)));
    } catch (err) {
      console.error('Error forfeiting pledge:', err);
      throw err;
    }
  };

  const cancelPledge = async (id: string) => {
    await deletePledge(id);
  };

  const deletePledge = async (id: string) => {
    try {
      await pledgeApi.delete(id);
      setPledges(prev => prev.filter(p => p.id !== id));
    } catch (err) {
      console.error('Error deleting pledge:', err);
      throw err;
    }
  };

  // ----------------------
  // SALE
  // ----------------------
  const sale = async (saleData: SaleData): Promise<boolean> => {
    try {
      const article = articles.find(a => a.id === saleData.articleId);
      if (!article || article.stock < saleData.quantity) {
        return false;
      }

      // Update article stock
      const updatedArticle = { ...article, stock: article.stock - saleData.quantity };
      await updateArticle(updatedArticle);

      // Record sale (this would need a Sale API endpoint)
      setSales(prev => [...prev, saleData]);
      return true;
    } catch (err) {
      console.error('Error processing sale:', err);
      return false;
    }
  };

  // ----------------------
  // PURCHASE
  // ----------------------
  const createPurchase = async (purchase: PurchaseRecord): Promise<boolean> => {
    try {
      // This would need a PurchaseRecord API endpoint
      setPurchases(prev => [...prev, purchase]);
      return true;
    } catch (err) {
      console.error('Error creating purchase:', err);
      return false;
    }
  };

  // Refresh data manually
  const refreshData = async () => {
    await loadData();
  };

  // ----------------------
  // PROVIDER
  // ----------------------
  return (
    <InventoryContext.Provider
      value={{
        articles,
        pledges,
        saleRecords: sales,
        clients,
        purchases,
        loading,
        error,
        addArticle,
        updateArticle,
        removeArticle,
        addClient,
        updateClient,
        deleteClient,
        createPledge,
        updatePledge,
        redeemPledge,
        forfeitPledge,
        cancelPledge,
        deletePledge,
        sale,
        createPurchase,
        getClients,
        refreshData,
      }}
    >
      {children}
    </InventoryContext.Provider>
  );
};

// ----------------------
// HOOK
// ----------------------
export const useInventory = () => {
  const context = useContext(InventoryContext);
  if (!context) throw new Error('useInventory must be used within an InventoryProvider');
  return context;
};
