import React, { useState, useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
import { ArrowUpTrayIcon, XCircleIcon, SparklesIcon } from '@heroicons/react/24/solid';
import { GoogleGenerativeAI, Part } from '@google/generative-ai';
import PulsingLoader from '../ui/PulsingLoader';

// Tipovi
interface GeminiValuationProps {
  onValuationComplete: (name: string, price: string, base64Images: string[]) => void;
}

// 1. Inicijalizacija Google AI klijenta s vašim API ključem iz .env.local datoteke
// UPOZORENJE: Ovaj ključ će biti vidljiv u kodu vaše javne web stranice.
const API_KEY = process.env.REACT_APP_GEMINI_API_KEY || "AIzaSyC41Q_2fVRWxcgyH3qSBFhfhuO7NJl1CWs";
if (!API_KEY) {
  console.error("Google AI API ključ nije pronađen. Molimo dodajte REACT_APP_GEMINI_API_KEY u .env.local datoteku.");
}
const genAI = new GoogleGenerativeAI(API_KEY);

// Pomoćna funkcija za konverziju Base64 stringa u format koji Gemini API razumije
function base64ToGenerativePart(base64String: string, mimeType: string): Part {
  return {
    inlineData: {
      data: base64String.split(',')[1], // Uklanjanje "data:image/jpeg;base64," prefiksa
      mimeType
    },
  };
}

const GeminiValuation: React.FC<GeminiValuationProps> = ({ onValuationComplete }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [previews, setPreviews] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);

  const onDrop = useCallback(async (acceptedFiles: File[]) => {
    if (acceptedFiles.length === 0) return;

    setIsLoading(true);
    setError(null);

    // Pretvaranje svake slike u Base64 format
    const base64Promises = acceptedFiles.map(file => {
      return new Promise<string>((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result as string);
        reader.onerror = error => reject(error);
      });
    });

    try {
      const base64Strings = await Promise.all(base64Promises);
      setPreviews(base64Strings);

      // --- DIREKTAN POZIV PREMA GOOGLE GEMINI API-JU ---
      const model = genAI.getGenerativeModel({ model: "gemini-pro-vision" });

      const prompt = "Na temelju ovih slika (prikazuju isti predmet iz više kutova), identificiraj predmet i daj mi kratak naziv za pretragu. Također, procijeni raspon cijena za rabljeni predmet u EUR. Formatiraj odgovor kao: Naziv: [naziv predmeta], Cijena: [raspon cijena].";
      
      const imageParts: Part[] = base64Strings.map(base64 => base64ToGenerativePart(base64, "image/jpeg"));

      const result = await model.generateContent([prompt, ...imageParts]);
      const response = await result.response;
      const text = response.text();
      // --- KRAJ API POZIVA ---

      // Parsiranje odgovora
      const name = text.split("Naziv:")[1]?.split("Cijena:")[0]?.trim() || "N/A";
      const price = text.split("Cijena:")[1]?.trim() || "N/A";
      
      onValuationComplete(name, price, base64Strings);
      
    } catch (err: any) {
      console.error("Greška pri komunikaciji s Gemini AI:", err);
      setError(err.message || 'Došlo je do greške prilikom analize. Provjerite API ključ i konzolu za detalje.');
    } finally {
      setIsLoading(false);
    }
  }, [onValuationComplete]);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: { 'image/jpeg': [], 'image/png': [] },
    multiple: true,
  });
  
  const removePreview = (index: number) => {
    setPreviews(prev => prev.filter((_, i) => i !== index));
  }

  return (
    <div className="bg-gray-50 p-4 rounded-lg border">
      <h3 className="text-lg font-semibold text-gray-800 mb-2 flex items-center">
        <SparklesIcon className="h-6 w-6 text-yellow-500 mr-2" />
        Automatska procjena (Gemini)
      </h3>
      
      {isLoading ? (
        <PulsingLoader text="Analiziram predmet..." />
      ) : (
        <>
          <div 
            {...getRootProps()} 
            className={`border-2 border-dashed rounded-lg p-6 text-center cursor-pointer transition-colors ${isDragActive ? 'border-blue-500 bg-blue-50' : 'border-gray-300 hover:border-gray-400'}`}
          >
            <input {...getInputProps()} />
            <ArrowUpTrayIcon className="mx-auto h-10 w-10 text-gray-400" />
            <p className="mt-2 text-sm text-gray-600">
              {previews.length > 0 ? 'Dodaj još slika...' : 'Povucite slike ili kliknite za odabir'}
            </p>
          </div>

          {previews.length > 0 && (
            <div className="mt-4 grid grid-cols-4 gap-2">
              {previews.map((src, index) => (
                <div key={index} className="relative">
                  <img src={src} alt={`Pregled ${index + 1}`} className="w-full h-20 object-cover rounded"/>
                  <button onClick={() => removePreview(index)} className="absolute -top-2 -right-2 bg-white rounded-full p-0.5">
                    <XCircleIcon className="h-6 w-6 text-red-500 hover:text-red-700"/>
                  </button>
                </div>
              ))}
            </div>
          )}
          {error && <p className="text-red-500 text-sm mt-2">{error}</p>}
        </>
      )}
    </div>
  );
};

export default GeminiValuation;