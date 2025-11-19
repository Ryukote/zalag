import React, { useState } from "react";

type ReportType =
  | "otkupni-blok"
  | "zahtjev-procjenu"
  | "medjuskladisnica"
  | "racun-o-isplati"
  | "izvjesce-o-prodaji-artikla"
  // Dodaj ostale po potrebi
  ;

interface ReportButtonProps {
  reportType: ReportType;
  articleId: string;
}

export function ReportButton({ reportType, articleId }: ReportButtonProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleDownload = async () => {
    setLoading(true);
    setError("");
    try {
      const token = localStorage.getItem("token");
      const response = await fetch(`/api/reports/${reportType}/${articleId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (!response.ok) throw new Error("Failed to download report");
      const blob = await response.blob();
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `${reportType}.pdf`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch (e: any) {
      setError(e.message || "Error while downloading report");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <button onClick={handleDownload} disabled={loading}>
        {loading ? "Učitavanje..." : `Preuzmi izvještaj: ${reportType}`}
      </button>
      {error && <div style={{ color: "red" }}>{error}</div>}
    </>
  );
}
