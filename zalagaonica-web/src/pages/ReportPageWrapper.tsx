import React from 'react';
import { useParams } from 'react-router-dom';
import { ReportButton } from '../services/Auth';

interface ReportPageWrapperProps {
  type: string;
}

export function ReportPageWrapper({ type }: ReportPageWrapperProps) {
  const { id } = useParams<{ id: string }>();
  if (!id) return <div>ID nije postavljen.</div>;

  return <ReportButton reportType={type as any} articleId={id} />;
}
