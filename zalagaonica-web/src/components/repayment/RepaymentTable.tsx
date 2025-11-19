import React from "react";

const RepaymentTable = ({ repayments, loading, onEdit, onDelete }: any) => {
  if (loading) return <p>Loading repayments...</p>;
  if (!repayments.length) return <p>No repayments found.</p>;

  return (
    <table className="min-w-full border border-gray-200 bg-white">
      <thead>
        <tr>
          <th className="border p-2">Loan ID</th>
          <th className="border p-2">Date</th>
          <th className="border p-2">Amount</th>
          <th className="border p-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        {repayments.map((repayment: any) => (
          <tr key={repayment.id}>
            <td className="border p-2">{repayment.loanId}</td>
            <td className="border p-2">{new Date(repayment.date).toLocaleDateString()}</td>
            <td className="border p-2">{repayment.amount.toFixed(2)}</td>
            <td className="border p-2 space-x-2">
              <button className="text-blue-600 hover:underline" onClick={() => onEdit(repayment)}>Edit</button>
              <button className="text-red-600 hover:underline" onClick={() => onDelete(repayment.id)}>Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default RepaymentTable;
