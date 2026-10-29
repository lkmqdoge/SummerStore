import type { InputHTMLAttributes } from "react";

interface Props extends InputHTMLAttributes<HTMLInputElement> {
    label: string;
}

export default function Field({ label, ...inputProps }: Props) {
    return (
        <label className="block text-sm font-medium">
            {label}
            <input
                {...inputProps}
                className="mt-1 w-full rounded border border-gray-300 px-3 py-2 focus:border-gray-900 focus:outline-none"
            />
        </label>
    );
}
