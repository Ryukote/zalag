import { useState, useEffect } from "react";
import { XMarkIcon } from "@heroicons/react/24/solid";
import { Client } from "../../services/clientApi";

interface ClientModalProps {
  isOpen: boolean;
  onClose: () => void;
  editingClientId?: string;
  editingClient?: Client;
  onSave: (client: Omit<Client, 'id' | 'createdAt' | 'updatedAt'> | Client) => Promise<void>;
}

export default function ClientModal({ isOpen, onClose, editingClientId, editingClient, onSave }: ClientModalProps) {
    const [name, setName] = useState("");
    const [city, setCity] = useState("");
    const [address, setAddress] = useState("");
    const [idCardNumber, setIdCardNumber] = useState("");
    const [email, setEmail] = useState("");
    const [iban, setIban] = useState("");
    const [type, setType] = useState<'legal' | 'individual'>('individual');
    const [status, setStatus] = useState<'active' | 'inactive'>('active');

    // Učitaj podatke ako se uređuje
    useEffect(() => {
        if (editingClient) {
            setName(editingClient.name);
            setCity(editingClient.city || "");
            setAddress(editingClient.address || "");
            setIdCardNumber(editingClient.idCardNumber || "");
            setEmail(editingClient.email || "");
            setIban(editingClient.iban || "");
            setType(editingClient.type);
            setStatus(editingClient.status);
        } else {
            setName("");
            setCity("");
            setAddress("");
            setIdCardNumber("");
            setEmail("");
            setIban("");
            setType('individual');
            setStatus('active');
        }
    }, [editingClient]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const now = new Date().toISOString();

        if (editingClient) {
            // Update existing client
            await onSave({
                ...editingClient,
                name,
                city,
                address,
                idCardNumber,
                email: email || undefined,
                iban: iban || '',
                type,
                status,
                updatedAt: now
            });
        } else {
            // Create new client
            await onSave({
                name,
                city,
                address,
                idCardNumber,
                email: email || undefined,
                iban: iban || '',
                type,
                status,
            });
        }

        onClose();
    };

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white rounded-lg shadow-xl w-full max-w-lg p-6 relative">
                <h2 className="text-xl font-bold mb-4">{editingClientId ? 'Uredi komitenta' : 'Novi komitent'}</h2>

                <form className="space-y-3" onSubmit={handleSubmit}>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Naziv</label>
                        <input value={name} onChange={e => setName(e.target.value)} type="text" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Grad</label>
                        <input value={city} onChange={e => setCity(e.target.value)} type="text" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Adresa</label>
                        <input value={address} onChange={e => setAddress(e.target.value)} type="text" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Broj osobne iskaznice</label>
                        <input value={idCardNumber} onChange={e => setIdCardNumber(e.target.value)} type="text" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Email</label>
                        <input value={email} onChange={e => setEmail(e.target.value)} type="email" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">IBAN</label>
                        <input value={iban} onChange={e => setIban(e.target.value)} type="text" className="mt-1 block w-full border rounded-md px-3 py-2" />
                    </div>
                    <div className="flex space-x-2">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Tip</label>
                            <select value={type} onChange={e => setType(e.target.value as 'legal' | 'individual')} className="mt-1 block w-full border rounded-md px-3 py-2">
                                <option value="individual">Individual</option>
                                <option value="legal">Legal</option>
                            </select>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Status</label>
                            <select value={status} onChange={e => setStatus(e.target.value as 'active' | 'inactive')} className="mt-1 block w-full border rounded-md px-3 py-2">
                                <option value="active">Active</option>
                                <option value="inactive">Inactive</option>
                            </select>
                        </div>
                    </div>

                    <div className="flex justify-end space-x-2 mt-4">
                        <button type="button" onClick={onClose} className="px-4 py-2 rounded-md bg-gray-200 hover:bg-gray-300">Odustani</button>
                        <button type="submit" className="px-4 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700">Spremi</button>
                    </div>
                </form>

                <button className="absolute top-3 right-3 text-gray-500 hover:text-gray-800" onClick={onClose}>
                    <XMarkIcon className="h-5 w-5" />
                </button>
            </div>
        </div>
    );
}
