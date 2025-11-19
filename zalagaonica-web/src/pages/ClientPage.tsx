import React, { useState, useRef, useEffect } from 'react';
import {
    PlusIcon,
    PrinterIcon,
    PencilIcon,
    CameraIcon,
    CurrencyDollarIcon,
    ShieldCheckIcon,
    CalendarDaysIcon,
    ArrowRightIcon,
    ArrowLeftIcon,
    CheckCircleIcon,
    UsersIcon,
    XMarkIcon,
    DocumentIcon,
    PhotoIcon,
    ArrowPathIcon,
    TicketIcon
} from '@heroicons/react/24/outline';
import Header from '../components/layout/Header';
import ClientModal from '../components/clients/ClientModal';
import { Pagination } from '../components/ui/Pagination';
import { clientApi, Client } from '../services/clientApi';
import { pledgeApi, CreatePledgeDto } from '../services/pledgeApi';
import * as PdfReportsApi from '../services/pdfReportsApi';

// --- FILE UPLOAD COMPONENT ---
interface FileUploadProps {
  acceptedFileTypes: string;
  title: string;
  files: File[];
  onFilesChange: (files: File[]) => void;
  icon: React.ElementType;
}

const FileUpload: React.FC<FileUploadProps> = ({ acceptedFileTypes, title, files, onFilesChange, icon: Icon }) => {
    const [isDragging, setIsDragging] = useState(false);
    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            const newFiles = Array.from(event.target.files);
            const allFiles = [...files, ...newFiles];
            onFilesChange(allFiles);
        }
    };

    const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setIsDragging(true);
    };

    const handleDragLeave = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setIsDragging(false);
    };

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        setIsDragging(false);
        if (event.dataTransfer.files) {
            const newFiles = Array.from(event.dataTransfer.files);
            const allFiles = [...files, ...newFiles];
            onFilesChange(allFiles);
        }
    };

    const handleRemoveFile = (index: number) => {
        const newFiles = files.filter((_, i) => i !== index);
        onFilesChange(newFiles);
    };

    return (
        <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{title}</label>
            <div
                onDragOver={handleDragOver}
                onDragLeave={handleDragLeave}
                onDrop={handleDrop}
                onClick={() => fileInputRef.current?.click()}
                className={`mt-1 flex flex-col items-center justify-center px-6 py-10 border-2 border-dashed rounded-md cursor-pointer transition-colors ${
                    isDragging ? 'border-indigo-500 bg-indigo-50' : 'border-gray-300 hover:border-gray-400'
                }`}
            >
                <div className="text-center">
                    <Icon className="mx-auto h-12 w-12 text-gray-400" />
                    <p className="mt-2 text-sm text-gray-600">
                        Povucite datoteke ili <span className="font-medium text-indigo-600">kliknite za odabir</span>
                    </p>
                    <p className="mt-1 text-xs text-gray-500">Dopušteni formati: {acceptedFileTypes.replace('image/*', 'Slike').replace('application/pdf', 'PDF')}</p>
                    <input
                        ref={fileInputRef}
                        type="file"
                        multiple
                        accept={acceptedFileTypes}
                        onChange={handleFileChange}
                        className="hidden"
                    />
                </div>
            </div>
            {files.length > 0 && (
                <div className="mt-4 space-y-2">
                    <h4 className="text-sm font-medium text-gray-600">Odabrane datoteke:</h4>
                    <ul className="divide-y divide-gray-200 border border-gray-200 rounded-md">
                        {files.map((file, index) => (
                            <li key={index} className="flex items-center justify-between p-2">
                                <div className="flex items-center min-w-0">
                                    {file.type.startsWith('image/') ? (
                                        <img src={URL.createObjectURL(file)} alt={file.name} className="h-10 w-10 rounded-md object-cover flex-shrink-0" />
                                    ) : (
                                        <DocumentIcon className="h-10 w-10 text-gray-400 flex-shrink-0" />
                                    )}
                                    <span className="ml-3 text-sm font-medium text-gray-800 truncate">{file.name}</span>
                                </div>
                                <button onClick={() => handleRemoveFile(index)} className="text-gray-400 hover:text-red-500 ml-2">
                                    <XMarkIcon className="h-5 w-5" />
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

// --- SHARED ITEM AND PLEDGE DATA INTERFACES ---
interface ItemData {
    name: string;
    description: string;
    estimatedValue: number;
}

interface PledgeData {
    loanAmount: number;
    returnAmount: number;
    period: number;
    dueDate: string;
}

// For purchase, we'll use a simpler structure since there's no return period
interface PurchaseData {
    purchaseAmount: number;
    totalAmount: number;
    paymentDate: string; // Today's date for immediate payment
}

// --- SHARED ITEM STEP COMPONENT ---
interface ItemStepProps {
  onBack: () => void;
  onNext: () => void;
  data: ItemData;
  onDataChange: (data: ItemData) => void;
  itemImages: File[];
  onItemImagesChange: (files: File[]) => void;
  warrantyFiles: File[];
  onWarrantyFilesChange: (files: File[]) => void;
  processType: 'pawn' | 'purchase';
}

const ItemStep: React.FC<ItemStepProps> = ({ 
  onBack, 
  onNext, 
  data, 
  onDataChange, 
  itemImages, 
  onItemImagesChange, 
  warrantyFiles, 
  onWarrantyFilesChange,
  processType 
}) => {
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        onDataChange({ ...data, [name]: value });
    };

    const handleValueChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseFloat(e.target.value);
        onDataChange({ ...data, estimatedValue: isNaN(value) ? 0 : value });
    };

    const stepTitle = processType === 'pawn' ? 'Korak 2: Podaci o predmetu za zalog' : 'Korak 2: Podaci o predmetu za otkup';

    return (
        <div>
            <h2 className="text-2xl font-semibold text-gray-800 mb-6">{stepTitle}</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Naziv predmeta</label>
                        <input name="name" value={data.name} onChange={handleInputChange} type="text" className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500" />
                    </div>
                     <div>
                        <label className="block text-sm font-medium text-gray-700">Opis i stanje</label>
                        <textarea name="description" value={data.description} onChange={handleInputChange} rows={4} className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"></textarea>
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Procjena vrijednosti (€)</label>
                        <div className="relative">
                           <CurrencyDollarIcon className="pointer-events-none absolute top-2.5 left-3 h-5 w-5 text-gray-400" />
                           <input value={data.estimatedValue === 0 ? '' : data.estimatedValue} onChange={handleValueChange} type="number" className="pl-10 mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500" />
                        </div>
                    </div>
                </div>
                <div className="space-y-6">
                    <FileUpload
                        title="Slike predmeta"
                        acceptedFileTypes="image/*"
                        files={itemImages}
                        onFilesChange={onItemImagesChange}
                        icon={PhotoIcon}
                    />
                    <FileUpload
                        title="Garancija (ako postoji)"
                        acceptedFileTypes="image/*,application/pdf"
                        files={warrantyFiles}
                        onFilesChange={onWarrantyFilesChange}
                        icon={ShieldCheckIcon}
                    />
                </div>
            </div>
            <div className="mt-8 flex justify-between">
                <button onClick={onBack} className="flex items-center bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors">
                    <ArrowLeftIcon className="h-5 w-5 mr-2" /> Natrag
                </button>
                <button onClick={onNext} className="flex items-center bg-indigo-600 text-white px-6 py-3 rounded-lg shadow-sm hover:bg-indigo-700 transition-colors">
                    Dalje <ArrowRightIcon className="h-5 w-5 ml-2" />
                </button>
            </div>
        </div>
    );
};

// --- PLEDGE STEP COMPONENT ---
interface PledgeStepProps {
  onBack: () => void;
  onNext: () => void;
  data: PledgeData;
  onDataChange: (data: PledgeData) => void;
}

const PledgeStep: React.FC<PledgeStepProps> = ({ onBack, onNext, data, onDataChange }) => {
    const calculateDueDate = (days: number): string => {
        const today = new Date();
        const dueDate = new Date(today.setDate(today.getDate() + days));
        return dueDate.toLocaleDateString('hr-HR');
    };

    const handleLoanAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseFloat(e.target.value);
        const loanAmount = isNaN(value) ? 0 : value;
        onDataChange({
            ...data,
            loanAmount,
            returnAmount: loanAmount * 1.10
        });
    };
    
    const handlePeriodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const period = parseInt(e.target.value, 10);
        onDataChange({
            ...data,
            period,
            dueDate: calculateDueDate(period)
        });
    };
    
    return (
        <div>
            <h2 className="text-2xl font-semibold text-gray-800 mb-6">Korak 3: Uvjeti zaloga</h2>
            <div className="space-y-6 max-w-md mx-auto">
                 <div>
                    <label className="block text-sm font-medium text-gray-700">Iznos pozajmice (€)</label>
                    <div className="relative mt-1">
                       <CurrencyDollarIcon className="pointer-events-none absolute top-2.5 left-3 h-5 w-5 text-gray-400" />
                       <input 
                           type="number"
                           value={data.loanAmount === 0 ? '' : data.loanAmount}
                           onChange={handleLoanAmountChange}
                           placeholder="0.00"
                           className="pl-10 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500" 
                       />
                    </div>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700">Period zaloga (dani)</label>
                    <select 
                        value={data.period}
                        onChange={handlePeriodChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                    >
                        <option value={30}>30 dana</option>
                        <option value={60}>60 dana</option>
                        <option value={90}>90 dana</option>
                    </select>
                </div>
                {data.loanAmount > 0 && (
                    <div className="p-4 bg-gray-100 rounded-lg">
                        <h3 className="font-semibold text-gray-800">Informativni izračun</h3>
                        <div className="mt-2 space-y-1 text-sm">
                            <p className="flex justify-between">Iznos za povrat: <span className="font-bold">{data.returnAmount.toFixed(2)} €</span></p>
                            <p className="flex justify-between">Datum dospijeća: <span className="font-bold">{data.dueDate}</span></p>
                        </div>
                    </div>
                )}
            </div>
            <div className="mt-8 flex justify-between">
                <button onClick={onBack} className="flex items-center bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors">
                    <ArrowLeftIcon className="h-5 w-5 mr-2" /> Natrag
                </button>
                <button onClick={onNext} className="flex items-center bg-indigo-600 text-white px-6 py-3 rounded-lg shadow-sm hover:bg-indigo-700 transition-colors">
                    Pregled i potvrda <ArrowRightIcon className="h-5 w-5 ml-2" />
                </button>
            </div>
        </div>
    );
};

// --- PURCHASE STEP COMPONENT ---
interface PurchaseStepProps {
  onBack: () => void;
  onNext: () => void;
  data: PurchaseData;
  onDataChange: (data: PurchaseData) => void;
}

const PurchaseStep: React.FC<PurchaseStepProps> = ({ onBack, onNext, data, onDataChange }) => {
    const today = new Date().toLocaleDateString('hr-HR');

    const handlePurchaseAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseFloat(e.target.value);
        const purchaseAmount = isNaN(value) ? 0 : value;
        onDataChange({
            ...data,
            purchaseAmount,
            totalAmount: purchaseAmount,
            paymentDate: today
        });
    };
    
    return (
        <div>
            <h2 className="text-2xl font-semibold text-gray-800 mb-6">Korak 3: Uvjeti otkupa</h2>
            <div className="space-y-6 max-w-md mx-auto">
                 <div>
                    <label className="block text-sm font-medium text-gray-700">Iznos otkupa (€)</label>
                    <div className="relative mt-1">
                       <CurrencyDollarIcon className="pointer-events-none absolute top-2.5 left-3 h-5 w-5 text-gray-400" />
                       <input 
                           type="number"
                           value={data.purchaseAmount === 0 ? '' : data.purchaseAmount}
                           onChange={handlePurchaseAmountChange}
                           placeholder="0.00"
                           className="pl-10 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500" 
                       />
                    </div>
                </div>
                {data.purchaseAmount > 0 && (
                    <div className="p-4 bg-gray-100 rounded-lg">
                        <h3 className="font-semibold text-gray-800">Informativni izračun</h3>
                        <div className="mt-2 space-y-1 text-sm">
                            <p className="flex justify-between">Ukupan iznos otkupa: <span className="font-bold">{data.totalAmount.toFixed(2)} €</span></p>
                            <p className="flex justify-between">Datum uplate: <span className="font-bold">{data.paymentDate}</span></p>
                            <p className="text-xs text-gray-600 mt-2">Napomena: Nakon uplate, predmet prelazi u vlasništvo zalagaonice.</p>
                        </div>
                    </div>
                )}
            </div>
            <div className="mt-8 flex justify-between">
                <button onClick={onBack} className="flex items-center bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors">
                    <ArrowLeftIcon className="h-5 w-5 mr-2" /> Natrag
                </button>
                <button onClick={onNext} className="flex items-center bg-indigo-600 text-white px-6 py-3 rounded-lg shadow-sm hover:bg-indigo-700 transition-colors">
                    Pregled i potvrda <ArrowRightIcon className="h-5 w-5 ml-2" />
                </button>
            </div>
        </div>
    );
};

// --- FORM DATA INTERFACES ---
interface PawnFormData {
    client: Client;
    item: ItemData;
    pledge: PledgeData;
    itemImages: File[];
    warrantyFiles: File[];
}

interface PurchaseFormData {
    client: Client;
    item: ItemData;
    purchase: PurchaseData;
    itemImages: File[];
    warrantyFiles: File[];
}

// --- CONFIRMATION STEP COMPONENT ---
interface ConfirmationStepProps {
  onBack: () => void;
  formData: PawnFormData | PurchaseFormData;
  onConfirm: () => void;
  processType: 'pawn' | 'purchase';
  onCreatePledge: (pledge: CreatePledgeDto) => Promise<void>;
  onCreatePurchase: (purchase: any) => Promise<void>;
}

const ConfirmationStep: React.FC<ConfirmationStepProps> = ({ onBack, formData, onConfirm, processType, onCreatePledge, onCreatePurchase }) => {
    const [isGenerating, setIsGenerating] = useState(false);

    const toBase64 = (file: Blob): Promise<string> =>
        new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result as string);
            reader.onerror = (error) => reject(error);
        });

    const handleGeneratePdf = async () => {
        setIsGenerating(true);
        try {
            // Convert images to base64
            const itemImagesBase64 = await Promise.all(
                formData.itemImages.map(file => toBase64(file))
            );
            const warrantyFilesBase64 = await Promise.all(
                formData.warrantyFiles.map(file => toBase64(file))
            );

            // Generate document ID
            const documentId = processType === 'pawn'
                ? `pledge-${Date.now()}`
                : `purchase-${Date.now()}`;

            // Save to backend
            if (processType === 'pawn') {
                const pawnData = formData as PawnFormData;
                const pledgeDto: CreatePledgeDto = {
                    clientId: formData.client.id,
                    clientName: formData.client.name,
                    itemName: pawnData.item.name,
                    itemDescription: pawnData.item.description,
                    estimatedValue: pawnData.item.estimatedValue,
                    loanAmount: pawnData.pledge.loanAmount,
                    returnAmount: pawnData.pledge.returnAmount,
                    period: pawnData.pledge.period,
                    pledgeDate: new Date().toISOString(),
                    redeemDeadline: new Date(Date.now() + pawnData.pledge.period * 24 * 60 * 60 * 1000).toISOString(),
                    itemImagesJson: JSON.stringify(itemImagesBase64),
                    warrantyFilesJson: JSON.stringify(warrantyFilesBase64)
                };
                await onCreatePledge(pledgeDto);
            } else {
                const purchaseData = formData as PurchaseFormData;
                const purchaseRecord: any = {
                    id: documentId,
                    clientId: formData.client.id,
                    clientName: formData.client.name,
                    itemName: purchaseData.item.name,
                    itemDescription: purchaseData.item.description,
                    estimatedValue: purchaseData.item.estimatedValue,
                    purchaseAmount: purchaseData.purchase.purchaseAmount,
                    totalAmount: purchaseData.purchase.totalAmount,
                    purchaseDate: new Date(),
                    paymentDate: purchaseData.purchase.paymentDate,
                    itemImages: itemImagesBase64,
                    warrantyFiles: warrantyFilesBase64
                };
                await onCreatePurchase(purchaseRecord);
            }

            // Generate PDF based on process type
            if (processType === 'purchase') {
                const purchaseData = formData as PurchaseFormData;
                const receiptData: PdfReportsApi.PurchaseReceiptData = {
                    documentNumber: documentId,
                    documentDate: new Date(),
                    seller: {
                        name: formData.client.name,
                        address: formData.client.address || '',
                        city: formData.client.city || '',
                        oib: formData.client.idCardNumber || ''
                    },
                    items: [{
                        name: purchaseData.item.name,
                        code: purchaseData.item.description || '-',
                        quantity: 1,
                        unitOfMeasure: 'KOM',
                        mpc: purchaseData.item.estimatedValue,
                        purchasePrice: purchaseData.purchase.purchaseAmount
                    }],
                    warehouse: 'Glavno skladište',
                    employeeName: 'Admin' // TODO: Get from logged in user
                };

                await PdfReportsApi.generatePurchaseReceipt(receiptData);
                onConfirm();
                setIsGenerating(false);
            } else {
                // For pawn documents
            const pawnData = formData as PawnFormData;
            const pledgeAgreementData: PdfReportsApi.PledgeAgreementData = {
                pledgeNumber: documentId,
                pledgeDate: new Date(),
                client: {
                    name: formData.client.name,
                    address: formData.client.address || '',
                    city: formData.client.city || '',
                    oib: formData.client.idCardNumber || ''
                },
                item: {
                    name: pawnData.item.name,
                    description: pawnData.item.description,
                    estimatedValue: pawnData.item.estimatedValue
                },
                loanAmount: pawnData.pledge.loanAmount,
                returnAmount: pawnData.pledge.returnAmount,
                period: pawnData.pledge.period,
                redeemDeadline: new Date(Date.now() + pawnData.pledge.period * 24 * 60 * 60 * 1000)
            };

                await PdfReportsApi.generatePledgeAgreement(pledgeAgreementData);
                onConfirm();
                setIsGenerating(false);
            }
        } catch (error) {
            console.error("Greška pri generiranju PDF-a:", error);
            alert("Došlo je do greške pri generiranju PDF-a. Molimo pokušajte ponovo.");
        } finally {
            setIsGenerating(false);
        }
    };

    const isPawnData = (data: PawnFormData | PurchaseFormData): data is PawnFormData => {
        return 'pledge' in data;
    };

     return (
        <div>
            <h2 className="text-2xl font-semibold text-gray-800 mb-6">Korak 4: Potvrda i spremanje</h2>
            <div className="bg-white p-6 rounded-lg shadow-md border border-gray-200 space-y-6">
                <div>
                    <h3 className="font-bold text-lg text-indigo-700 border-b pb-2 mb-3">Podaci o klijentu</h3>
                    <p><span className="font-semibold">Ime i prezime:</span> {formData.client.name}</p>
                    <p><span className="font-semibold">Adresa:</span> {formData.client.address}</p>
                </div>
                <div>
                    <h3 className="font-bold text-lg text-indigo-700 border-b pb-2 mb-3">Podaci o predmetu</h3>
                    <p><span className="font-semibold">Naziv:</span> {formData.item.name}</p>
                    <p><span className="font-semibold">Procjena:</span> {formData.item.estimatedValue.toFixed(2)} €</p>
                    <p><span className="font-semibold">Broj slika:</span> {formData.itemImages.length}</p>
                    <p><span className="font-semibold">Garancija:</span> {formData.warrantyFiles.length > 0 ? 'Priložena' : 'Nije priložena'}</p>
                </div>
                <div>
                    <h3 className="font-bold text-lg text-indigo-700 border-b pb-2 mb-3">
                        {processType === 'pawn' ? 'Uvjeti zaloga' : 'Uvjeti otkupa'}
                    </h3>
                    {isPawnData(formData) ? (
                        <>
                            <p><span className="font-semibold">Iznos pozajmice:</span> {formData.pledge.loanAmount.toFixed(2)} €</p>
                            <p><span className="font-semibold">Iznos za povrat:</span> {formData.pledge.returnAmount.toFixed(2)} €</p>
                            <p><span className="font-semibold">Datum dospijeća:</span> {formData.pledge.dueDate}</p>
                        </>
                    ) : (
                        <>
                            <p><span className="font-semibold">Iznos otkupa:</span> {formData.purchase.purchaseAmount.toFixed(2)} €</p>
                            <p><span className="font-semibold">Ukupan iznos:</span> {formData.purchase.totalAmount.toFixed(2)} €</p>
                            <p><span className="font-semibold">Datum uplate:</span> {formData.purchase.paymentDate}</p>
                            <p className="text-sm text-gray-600"><span className="font-semibold">Napomena:</span> Predmet prelazi u vlasništvo zalagaonice nakon uplate</p>
                        </>
                    )}
                </div>
            </div>
            <div className="mt-8 flex justify-between">
                <button onClick={onBack} className="flex items-center bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors">
                    <ArrowLeftIcon className="h-5 w-5 mr-2" /> Natrag
                </button>
                <button 
                    onClick={handleGeneratePdf} 
                    disabled={isGenerating}
                    className="flex items-center justify-center bg-green-600 text-white px-6 py-3 rounded-lg shadow-sm hover:bg-green-700 transition-colors disabled:bg-green-400 disabled:cursor-not-allowed"
                >
                    {isGenerating ? (
                        <>
                            <ArrowPathIcon className="animate-spin h-5 w-5 mr-2" />
                            Generiranje...
                        </>
                    ) : (
                        <>
                            <CheckCircleIcon className="h-5 w-5 mr-2" />
                            Potvrdi i kreiraj ugovor
                        </>
                    )}
                </button>
            </div>
        </div>
    );
};

// --- PROCESS PAGE COMPONENTS ---
interface ProcessPageProps {
  client: Client;
  onClose: () => void;
  processType: 'pawn' | 'purchase';
  onCreatePledge: (pledge: CreatePledgeDto) => Promise<void>;
  onCreatePurchase: (purchase: any) => Promise<void>;
}

const ProcessPage: React.FC<ProcessPageProps> = ({ client, onClose, processType, onCreatePledge, onCreatePurchase }) => {
    const [currentStep, setCurrentStep] = useState(1);
    const [pawnFormData, setPawnFormData] = useState<PawnFormData>({ 
        client,
        item: { name: '', description: '', estimatedValue: 0 },
        pledge: { loanAmount: 0, returnAmount: 0, period: 30, dueDate: new Date(new Date().setDate(new Date().getDate() + 30)).toLocaleDateString('hr-HR') },
        itemImages: [],
        warrantyFiles: []
    });
    const [purchaseFormData, setPurchaseFormData] = useState<PurchaseFormData>({ 
        client,
        item: { name: '', description: '', estimatedValue: 0 },
        purchase: { purchaseAmount: 0, totalAmount: 0, paymentDate: new Date().toLocaleDateString('hr-HR') },
        itemImages: [],
        warrantyFiles: []
    });

    const handlePawnDataChange = (newData: Partial<PawnFormData>) => {
        setPawnFormData(prev => ({ ...prev, ...newData }));
    };

    const handlePurchaseDataChange = (newData: Partial<PurchaseFormData>) => {
        setPurchaseFormData(prev => ({ ...prev, ...newData }));
    };

    const handleBack = () => {
        if (currentStep === 1) {
            onClose();
        } else {
            setCurrentStep(prev => prev - 1);
        }
    };
    
    const handleNext = () => {
        setCurrentStep(prev => prev + 1);
    }

    const handleConfirm = () => {
        onClose();
    }

    const renderStep = () => {
        const formData = processType === 'pawn' ? pawnFormData : purchaseFormData;
        
        switch (currentStep) {
            case 1:
                return <ItemStep 
                         onBack={handleBack} 
                         onNext={handleNext} 
                         data={formData.item}
                         onDataChange={(item) => {
                             if (processType === 'pawn') {
                                 handlePawnDataChange({ item });
                             } else {
                                 handlePurchaseDataChange({ item });
                             }
                         }}
                         itemImages={formData.itemImages}
                         onItemImagesChange={(itemImages) => {
                             if (processType === 'pawn') {
                                 handlePawnDataChange({ itemImages });
                             } else {
                                 handlePurchaseDataChange({ itemImages });
                             }
                         }}
                         warrantyFiles={formData.warrantyFiles}
                         onWarrantyFilesChange={(warrantyFiles) => {
                             if (processType === 'pawn') {
                                 handlePawnDataChange({ warrantyFiles });
                             } else {
                                 handlePurchaseDataChange({ warrantyFiles });
                             }
                         }}
                         processType={processType}
                       />;
            case 2:
                if (processType === 'pawn') {
                    return <PledgeStep 
                             onBack={handleBack} 
                             onNext={handleNext} 
                             data={pawnFormData.pledge}
                             onDataChange={(pledge) => handlePawnDataChange({ pledge })}
                           />;
                } else {
                    return <PurchaseStep 
                             onBack={handleBack} 
                             onNext={handleNext} 
                             data={purchaseFormData.purchase}
                             onDataChange={(purchase) => handlePurchaseDataChange({ purchase })}
                           />;
                }
            case 3:
                return <ConfirmationStep
                         onBack={handleBack}
                         formData={formData}
                         onConfirm={handleConfirm}
                         processType={processType}
                         onCreatePledge={onCreatePledge}
                         onCreatePurchase={onCreatePurchase}
                       />;
            default:
                return <div>Greška: Nepoznat korak.</div>;
        }
    };

    const pageTitle = processType === 'pawn' ? `Novi zalog za: ${client.name}` : `Novi otkup za: ${client.name}`;
    const pageDescription = processType === 'pawn' ? 'Vodite proces zaloga korak po korak' : 'Vodite proces otkupa korak po korak';

    return (
        <div className="px-6 py-8 bg-gray-50 min-h-screen">
            <header className="bg-white shadow-sm mb-8 -mx-6 -mt-8 px-6 py-4">
                <div className="flex justify-between items-center max-w-7xl mx-auto">
                    <div>
                        <h1 className="text-2xl font-bold leading-6 text-gray-900">{pageTitle}</h1>
                        <p className="text-sm text-gray-500 mt-1">{pageDescription}</p>
                    </div>
                    <button onClick={onClose} className="text-gray-500 hover:text-gray-800">
                        <XMarkIcon className="h-6 w-6" />
                    </button>
                </div>
            </header>
            <main>
                <div className="max-w-5xl mx-auto">
                    <div className="mb-12">
                         <ol className="flex items-center w-full">
                             <li className="flex w-full items-center text-indigo-600 after:content-[''] after:w-full after:h-1 after:border-b after:border-indigo-600 after:border-4 after:inline-block">
                                <span className="flex items-center justify-center w-10 h-10 bg-indigo-100 rounded-full lg:h-12 lg:w-12 shrink-0">
                                    <UsersIcon className="w-6 h-6"/>
                                </span>
                            </li>
                            <li className={`flex w-full items-center ${currentStep >= 1 ? 'text-indigo-600' : 'text-gray-500'} after:content-[''] after:w-full after:h-1 after:border-b ${currentStep > 1 ? 'after:border-indigo-600' : 'after:border-gray-300'} after:border-4 after:inline-block`}>
                                <span className={`flex items-center justify-center w-10 h-10 ${currentStep >= 1 ? 'bg-indigo-100' : 'bg-gray-100'} rounded-full lg:h-12 lg:w-12 shrink-0`}>
                                    <CameraIcon className="w-6 h-6"/>
                                </span>
                            </li>
                            <li className={`flex w-full items-center ${currentStep >= 2 ? 'text-indigo-600' : 'text-gray-500'} after:content-[''] after:w-full after:h-1 after:border-b ${currentStep > 2 ? 'after:border-indigo-600' : 'after:border-gray-300'} after:border-4 after:inline-block`}>
                                <span className={`flex items-center justify-center w-10 h-10 ${currentStep >= 2 ? 'bg-indigo-100' : 'bg-gray-100'} rounded-full lg:h-12 lg:w-12 shrink-0`}>
                                    <CalendarDaysIcon className="w-6 h-6"/>
                                </span>
                            </li>
                            <li className={`flex items-center ${currentStep >= 3 ? 'text-indigo-600' : 'text-gray-500'}`}>
                                <span className={`flex items-center justify-center w-10 h-10 ${currentStep >= 3 ? 'bg-indigo-100' : 'bg-gray-100'} rounded-full lg:h-12 lg:w-12 shrink-0`}>
                                    <CheckCircleIcon className="w-6 h-6"/>
                                </span>
                            </li>
                        </ol>
                    </div>
                    <div className="bg-white p-8 rounded-xl shadow-lg">
                        {renderStep()}
                    </div>
                </div>
            </main>
        </div>
    );
};

// --- CLIENT TABLE COMPONENT ---
interface ClientTableProps {
  data: Client[];
  onEdit: (client: Client) => void;
  onStartPawn: (client: Client) => void;
  onStartPurchase: (client: Client) => void;
}

const ClientTable: React.FC<ClientTableProps> = ({ data, onEdit, onStartPawn, onStartPurchase }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-gray-900 sm:pl-6">Naziv</th>
                    <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Grad</th>
                    <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Broj osobne</th>
                    <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Status</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-4 sm:pr-6 text-right">Akcije</th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.map((client: Client) => (
                    <tr key={client.id}>
                        <td className="whitespace-nowrap py-4 pl-4 pr-3 text-sm font-medium text-gray-900 sm:pl-6">{client.name}</td>
                        <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">{client.city}</td>
                        <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">{client.idCardNumber}</td>
                        <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                             <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${client.status === 'active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                                {client.status === 'active' ? 'Aktivan' : 'Neaktivan'}
                            </span>
                        </td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-6 space-x-2">
                            <button onClick={() => onStartPawn(client)} className="text-blue-600 hover:text-blue-900" title="Novi zalog">
                                <TicketIcon className="h-5 w-5" />
                            </button>
                            <button onClick={() => onStartPurchase(client)} className="text-green-600 hover:text-green-900" title="Novi otkup">
                                <CurrencyDollarIcon className="h-5 w-5" />
                            </button>
                            <button onClick={() => onEdit(client)} className="text-gray-500 hover:text-gray-800" title="Uredi komitenta">
                                <PencilIcon className="h-5 w-5" />
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

// --- MAIN PAGE COMPONENT ---
const ClientPage: React.FC = () => {
    // Data state
    const [clients, setClients] = useState<Client[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [activeProcess, setActiveProcess] = useState<'none' | 'pawn' | 'purchase'>('none');
    const [selectedClient, setSelectedClient] = useState<Client | null>(null);

    // Pagination
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(10);

    const currentItems = clients.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    const handleItemsPerPageChange = (value: number) => {
        setItemsPerPage(value);
        setCurrentPage(1);
    };

    // Modal states
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingClient, setEditingClient] = useState<Client | null>(null);

    // Load clients from backend
    const loadClients = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await clientApi.getAll();
            setClients(data);
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to load clients';
            setError(message);
            console.error('Error loading clients:', err);
        } finally {
            setLoading(false);
        }
    };

    // Load data on mount
    useEffect(() => {
        loadClients();
    }, []);

    // -----------------------------
    // ADD / EDIT
    // -----------------------------
    const handleOpenAddModal = () => {
        setEditingClient(null);
        setIsModalOpen(true);
    };

    const handleOpenEditModal = (client: Client) => {
        setEditingClient(client);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingClient(null);
    };

    const handleSaveClient = async (clientData: Omit<Client, 'id' | 'createdAt' | 'updatedAt'> | Client) => {
        try {
            setLoading(true);

            if ('id' in clientData && clientData.id) {
                // Update existing client
                await clientApi.update(clientData.id, clientData as Client);
                setClients(prev => prev.map(c => c.id === clientData.id ? clientData as Client : c));
                alert('Komitent uspješno ažuriran!');
            } else {
                // Create new client
                const newClient = await clientApi.create(clientData);
                setClients(prev => [...prev, newClient]);
                alert('Komitent uspješno dodan!');
            }
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to save client';
            alert('Greška pri spremanju: ' + message);
            console.error('Error saving client:', err);
        } finally {
            setLoading(false);
        }
    };

    // -----------------------------
    // PLEDGE / PURCHASE HANDLERS
    // -----------------------------
    const handleCreatePledge = async (pledge: CreatePledgeDto) => {
        try {
            setLoading(true);
            await pledgeApi.create(pledge);
            alert('Zalog uspješno kreiran!');
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to create pledge';
            alert('Greška pri kreiranju zaloga: ' + message);
            console.error('Error creating pledge:', err);
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const handleCreatePurchase = async (purchase: any) => {
        try {
            setLoading(true);
            // TODO: Implement purchase record API
            console.log('Purchase record (API not implemented yet):', purchase);
            alert('Otkup evidentiran! (Napomena: Backend API za otkup još nije implementiran)');
        } catch (err) {
            const message = err instanceof Error ? err.message : 'Failed to create purchase';
            alert('Greška pri kreiranju otkupa: ' + message);
            console.error('Error creating purchase:', err);
            throw err;
        } finally {
            setLoading(false);
        }
    };

    // -----------------------------
    // PROCESS HANDLERS
    // -----------------------------
    const handleStartPawnProcess = (client: Client) => {
        setSelectedClient(client);
        setActiveProcess('pawn');
    };

    const handleStartPurchaseProcess = (client: Client) => {
        setSelectedClient(client);
        setActiveProcess('purchase');
    };

    const handleCloseProcess = () => {
        setActiveProcess('none');
        setSelectedClient(null);
    };
    
    if (activeProcess !== 'none' && selectedClient) {
        return <ProcessPage
                 client={selectedClient}
                 onClose={handleCloseProcess}
                 processType={activeProcess}
                 onCreatePledge={handleCreatePledge}
                 onCreatePurchase={handleCreatePurchase}
               />;
    }

    if (loading && clients.length === 0) {
        return (
            <div className="px-6 py-8 bg-gray-50 min-h-screen flex items-center justify-center">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Učitavanje komitenata...</p>
                </div>
            </div>
        );
    }

    if (error && clients.length === 0) {
        return (
            <div className="px-6 py-8 bg-gray-50 min-h-screen">
                <div className="bg-red-50 border border-red-200 rounded-md p-4">
                    <p className="text-red-800">Greška: {error}</p>
                    <button
                        onClick={loadClients}
                        className="mt-2 text-sm text-red-600 hover:text-red-800 underline"
                    >
                        Pokušaj ponovno
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="px-6 py-8 bg-gray-50 min-h-screen">
            <Header title="Komitenti" showBackButton={true} />

            {error && (
                <div className="mt-4 bg-yellow-50 border border-yellow-200 rounded-md p-4">
                    <p className="text-yellow-800">Upozorenje: {error}</p>
                </div>
            )}

            <div className="mt-6 md:flex md:items-center md:justify-between">
                <div className="flex-1 min-w-0">
                    <div className="flex items-center space-x-4 text-sm text-gray-600">
                        <span>Ukupno komitenata: {clients.length}</span>
                        <span className="text-green-600">
                            Aktivnih: {clients.filter(c => c.status === 'active').length}
                        </span>
                        <span className="text-red-600">
                            Neaktivnih: {clients.filter(c => c.status === 'inactive').length}
                        </span>
                    </div>
                </div>

                <div className="mt-4 flex md:mt-0 md:ml-4">
                    <button
                        type="button"
                        className="flex items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                    >
                        <PrinterIcon className="h-5 w-5 mr-2 text-gray-500" />
                        Ispis Liste
                    </button>
                    <button
                        type="button"
                        onClick={handleOpenAddModal}
                        disabled={loading}
                        className="ml-3 flex items-center justify-center rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        <PlusIcon className="h-5 w-5 mr-2" />
                        Novi Komitent
                    </button>
                </div>
            </div>

            <div className="mt-8 flow-root">
                <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                    <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                        <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded-lg">
                            {loading && clients.length > 0 && (
                                <div className="bg-blue-50 px-4 py-2 text-sm text-blue-700">
                                    Obrada...
                                </div>
                            )}

                            <ClientTable
                                data={currentItems}
                                onEdit={handleOpenEditModal}
                                onStartPawn={handleStartPawnProcess}
                                onStartPurchase={handleStartPurchaseProcess}
                            />
                        </div>

                        <Pagination
                            currentPage={currentPage}
                            totalCount={clients.length}
                            itemsPerPage={itemsPerPage}
                            onPageChange={setCurrentPage}
                            onItemsPerPageChange={handleItemsPerPageChange}
                        />
                    </div>
                </div>
            </div>

            {/* MODAL: Add/Edit */}
            <ClientModal
                isOpen={isModalOpen}
                onClose={handleCloseModal}
                editingClientId={editingClient?.id}
                editingClient={editingClient || undefined}
                onSave={handleSaveClient}
            />
        </div>
    );
};

export default ClientPage;