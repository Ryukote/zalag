import React, { useEffect, useState } from "react";
import axios from "axios";
import RepaymentModal from "../components/repayment/RepaymentModal";
import RepaymentTable from "../components/repayment/RepaymentTable";

const RepaymentPage: React.FC = () => {
  const [repayments, setRepayments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedRepayment, setSelectedRepayment] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchRepayments = async () => {
    setLoading(true);
    try {
      const response = await axios.get("/api/repayment");
      setRepayments(response.data);
    } catch (e) {
      console.error("Error fetching repayments", e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRepayments();
  }, []);

  const onEdit = (repayment: any) => {
    setSelectedRepayment(repayment);
    setModalOpen(true);
  };

  const onDelete = async (id: any) => {
    if (window.confirm("Are you sure to delete this repayment?")) {
      try {
        await axios.delete(`/api/repayment/${id}`);
        fetchRepayments();
      } catch (e) {
        console.error("Error deleting repayment", e);
      }
    }
  };

  const onSave = async (repayment: any) => {
    try {
      if (repayment.id) {
        await axios.put(`/api/repayment/${repayment.id}`, repayment);
      } else {
        await axios.post("/api/repayment", repayment);
      }
      fetchRepayments();
      setModalOpen(false);
      setSelectedRepayment(null);
    } catch (e) {
      console.error("Error saving repayment", e);
    }
  };

  const onClose = () => {
    setModalOpen(false);
    setSelectedRepayment(null);
  };

  return (
    <div className="p-4">
      <h1 className="font-semibold text-xl mb-4">Repayments</h1>
      <button
        className="mb-4 px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
        onClick={() => setModalOpen(true)}
      >
        Add Repayment
      </button>

      <RepaymentTable repayments={repayments} loading={loading} onEdit={onEdit} onDelete={onDelete} />

      {modalOpen && (
        <RepaymentModal repayment={selectedRepayment} onSave={onSave} onClose={onClose} />
      )}
    </div>
  );
};

export default RepaymentPage;
