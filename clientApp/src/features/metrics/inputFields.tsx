import {
  TextField,
  Dropdown,
  IDropdownOption,
  IDropdownStyleProps,
  IDropdownStyles,
  IRenderFunction,
  IStyleFunctionOrObject,
  ITextFieldProps,
  ITextFieldStyleProps,
  ITextFieldStyles,
} from "@fluentui/react";
import React from 'react';
import { useField, useFormikContext } from "formik";

export interface InputPropsInterface {
  label: string;
  name: string;
  type: string;
  placeholder?: string;
  id?: string;
  required?: boolean;
  readOnly?: boolean;
}
export interface TextInputPropsInterface extends InputPropsInterface {
  styles?: IStyleFunctionOrObject<ITextFieldStyleProps, ITextFieldStyles>;
  multiline?: boolean;
  rows?: number;
  onRenderLabel?: IRenderFunction<ITextFieldProps>;
  onSuccessRenderDescription?: IRenderFunction<ITextFieldProps>;
}

export interface DropdownInputPropsInterface extends InputPropsInterface {
  styles?: IStyleFunctionOrObject<IDropdownStyleProps, IDropdownStyles>;
  options: IDropdownOption[];
}

export const TextInput = ({ label, ...props }: TextInputPropsInterface) => {
  const [field, meta] = useField(props);
  return (
    <>
      {meta.touched && meta.error ? (
        <TextField
          label={label}
          {...field}
          {...props}
          errorMessage={meta.error}
        />
      ) : (
        <TextField label={label} {...field} {...props} />
      )}
    </>
  );
};

export const DropdownInput = ({
  label,
  ...props
}: DropdownInputPropsInterface) => {
  const [field, meta] = useField(props);
  const { setFieldValue } = useFormikContext();
  return (
    <>
      {meta.touched && meta.error ? (
        <Dropdown
          label={label}
          {...field}
          {...props}
          onChange={(_, value) => setFieldValue(props.name, value?.key)}
          errorMessage={meta.error}
        />
      ) : (
        <Dropdown
          label={label}
          {...field}
          {...props}
          onChange={(_, value) => setFieldValue(props.name, value?.key)}
        />
      )}
    </>
  );
};
