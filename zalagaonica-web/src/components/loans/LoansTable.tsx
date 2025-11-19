import React from "react";

const LoansTable = ({ loans, loading, onEdit, onDelete }: any) => {
  if (loading) return <p>Loading loans...</p>;
  if (!loans.length) return <p>No loans found.</p>;

  return (
    <table className="min-w-full border border-gray-200 bg-white">
      <thead>
        <tr>
          <th className="border p-2">Client</th>
          <th className="border p-2">Date</th>
          <th className="border p-2">Amount</th>
          <th className="border p-2">Interest</th>
          <th className="border p-2">Due Date</th>
          <th className="border p-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        {loans.map((loan: any) => (
          <tr key={loan.id}>
            <td className="border p-2">{loan.clientName}</td>
            <td className="border p-2">{new Date(loan.date).toLocaleDateString()}</td>
            <td className="border p-2">{loan.amount.toFixed(2)}</td>
            <td className="border p-2">{loan.interest.toFixed(2)}%</td>
            <td className="border p-2">{new Date(loan.dueDate).toLocaleDateString()}</td>
            <td className="border p-2 space-x-2">
              <button className="text-blue-600 hover:underline" onClick={() => onEdit(loan)}>Edit</button>
              <button className="text-red-600 hover:underline" onClick={() => onDelete(loan.id)}>Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default LoansTable;
