/* eslint-disable no-console */
import { Panel, PrimaryButton } from "@fluentui/react";
import { Form, Formik } from "formik";
import React, { useEffect } from "react";
import * as Yup from "yup";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { DropdownInput, TextInput } from "./inputFields";
import {
  addMetric,
  editMetric,
  isNewMetricCustomization,
  selectMetricCustomizationPanelDetails,
  selectPortfolioMetrics,
  selectUserUnselectedMetrics,
  updateMetricCustomizationPanel,
} from "./metricsSlice";

function MetricsCustomization() {
  const dispatch = useAppDispatch();
  const panelDetails = useAppSelector(selectMetricCustomizationPanelDetails);
  const unselectedMetrics = useAppSelector(selectUserUnselectedMetrics);
  const portfolioMetrics = useAppSelector(selectPortfolioMetrics);

  const getPanelHeader = (): string =>
    panelDetails.metric ? "Edit Metric" : "Add Metric";
  const getInitialValues = () => {
    if (!panelDetails.metric) {
      return {
        name: "",
        upperThreshold: "",
        lowerThreshold: "",
      };
    } else {
      return {
        name: panelDetails.metric,
        upperThreshold:
          portfolioMetrics.find((m) => m.name === panelDetails.metric)
            ?.upperThreshold || "",
        lowerThreshold:
          portfolioMetrics.find((m) => m.name === panelDetails.metric)
            ?.lowerThreshold || "",
      };
    }
  };

  const getDropdownOptions = () => {
    if (panelDetails.metric)
      return [{ key: panelDetails.metric, text: panelDetails.metric }];
    return unselectedMetrics.map((key) => {
      return { key, text: key };
    });
  };

  return (
    <Panel
      headerText={getPanelHeader()}
      isOpen={panelDetails.isOpen}
      onDismiss={() =>
        dispatch(updateMetricCustomizationPanel({ isOpen: false }))
      }
      closeButtonAriaLabel="Close"
    >
      <Formik
        validateOnChange={false}
        enableReinitialize
        initialValues={getInitialValues()}
        initialErrors={{
          name: "",
          upperThreshold: "",
          lowerThreshold: "",
        }}
        validationSchema={Yup.object({
          name: Yup.string().default("").required("Required"),
          upperThreshold: Yup.string()
            .default("")
            .required("Required")
            .matches(
              /^(?!-0?(\.0+)?$)-?(0|[1-9]\d*)?(\.\d+)?(?<=\d)$/,
              "Provide valid number"
            ),
          lowerThreshold: Yup.string()
            .default("")
            .required("Required")
            .matches(
              /^(?!-0?(\.0+)?$)-?(0|[1-9]\d*)?(\.\d+)?(?<=\d)$/,
              "Provide valid number"
            ),
        })}
        onSubmit={async (values, { setSubmitting }) => {
          //   console.log(values);
          if (panelDetails.metric) dispatch(editMetric(values));
          else dispatch(addMetric(values));
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
                disabled={panelDetails.metric ? true : false}
                type="select"
                label="Metric"
                name="name"
                defaultSelectedKey={
                  panelDetails.metric ? panelDetails.metric : undefined
                }
                placeholder={
                  panelDetails.metric ? undefined : "Select an Option"
                }
                options={getDropdownOptions()}
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
            <PrimaryButton text={getPanelHeader()} type="submit" />
          </div>
        </Form>
      </Formik>
    </Panel>
  );
}

export default MetricsCustomization;
