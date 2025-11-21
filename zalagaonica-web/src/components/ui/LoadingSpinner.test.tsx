import { render, screen } from '@testing-library/react';
import LoadingSpinner from './LoadingSpinner';

describe('LoadingSpinner', () => {
  it('renders without crashing', () => {
    render(<LoadingSpinner />);
    const spinnerElement = screen.getByRole('status');
    expect(spinnerElement).toBeInTheDocument();
  });

  it('displays the loading message when provided', () => {
    const message = 'Učitavanje podataka...';
    render(<LoadingSpinner message={message} />);
    expect(screen.getByText(message)).toBeInTheDocument();
  });

  it('does not display message when not provided', () => {
    const { container } = render(<LoadingSpinner />);
    const messageElement = container.querySelector('.mt-3');
    expect(messageElement).not.toBeInTheDocument();
  });

  it('renders as fullscreen when fullScreen prop is true', () => {
    const { container } = render(<LoadingSpinner fullScreen={true} />);
    const fullScreenDiv = container.querySelector('.fixed.inset-0');
    expect(fullScreenDiv).toBeInTheDocument();
  });

  it('renders inline when fullScreen prop is false', () => {
    const { container } = render(<LoadingSpinner fullScreen={false} />);
    const inlineDiv = container.querySelector('.flex.justify-center');
    expect(inlineDiv).toBeInTheDocument();
  });

  it('applies correct size classes for small size', () => {
    const { container } = render(<LoadingSpinner size="small" />);
    const spinner = container.querySelector('.h-6.w-6');
    expect(spinner).toBeInTheDocument();
  });

  it('applies correct size classes for medium size', () => {
    const { container } = render(<LoadingSpinner size="medium" />);
    const spinner = container.querySelector('.h-12.w-12');
    expect(spinner).toBeInTheDocument();
  });

  it('applies correct size classes for large size', () => {
    const { container } = render(<LoadingSpinner size="large" />);
    const spinner = container.querySelector('.h-16.w-16');
    expect(spinner).toBeInTheDocument();
  });

  it('has correct ARIA attributes for accessibility', () => {
    render(<LoadingSpinner />);
    const spinnerElement = screen.getByRole('status');
    expect(spinnerElement).toHaveAttribute('aria-live', 'polite');
  });
});
