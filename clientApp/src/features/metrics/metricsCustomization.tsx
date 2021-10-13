/* eslint-disable no-console */
import { Panel, PrimaryButton } from "@fluentui/react";
import { Form, Formik } from "formik";
import React, { useEffect } from "react";
import * as Yup from "yup";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { DropdownInput, TextInput } from "./inputFields";
import {
  addMetric,
  selectMetricCustomizationPanelDetails,
  selectUserUnselectedMetrics,
  updateMetricCustomizationPanel,
} from "./metricsSlice";

function MetricsCustomization() {
  const dispatch = useAppDispatch();
  const panelDetails = useAppSelector(selectMetricCustomizationPanelDetails);
  const unselectedMetrics = useAppSelector(selectUserUnselectedMetrics);
  useEffect(() => {
    console.log("gg");
    console.log(unselectedMetrics);
  });
  return (
    <Panel
      headerText="Add Metric"
      isOpen={panelDetails.isOpen}
      onDismiss={() =>
        dispatch(updateMetricCustomizationPanel({ isOpen: false }))
      }
      closeButtonAriaLabel="Close"
    >
      <Formik
        validateOnChange={false}
        enableReinitialize
        initialValues={{
          name: "",
          upperThreshold: "",
          lowerThreshold: "",
        }}
        initialErrors={{
          name: "",
          upperThreshold: "",
          lowerThreshold: "",
        }}
        validationSchema={Yup.object({
          name: Yup.string().default("").required("Required"),
          upperThreshold: Yup.string().default("").required("Required"),
          lowerThreshold: Yup.string().default("").required("Required"),
        })}
        onSubmit={async (values, { setSubmitting }) => {
          dispatch(addMetric(values));
        }}
      >
        <Form>
          <div
            style={{
              display: "flex",
              flexWrap: "wrap",
              flexDirection: "column",
              minHeight: "90vh",
              justifyContent: "space-between",
            }}
          >
            <div>
              <DropdownInput
                required
                type="select"
                label="Metric"
                name="name"
                placeholder="Select an Option"
                options={unselectedMetrics.map((key) => {
                  return { key, text: key };
                })}
              />
              <TextInput
                required
                id="lowerThreshold"
                label="Lower Threshold"
                name="lowerThreshold"
                type="text"
              />
              <TextInput
                required
                id="upperThreshold"
                label="Upper Threshold"
                name="upperThreshold"
                type="text"
              />
            </div>
            <PrimaryButton text="Add Metric" type="submit" />
          </div>
        </Form>
      </Formik>
    </Panel>
  );
}

export default MetricsCustomization;
