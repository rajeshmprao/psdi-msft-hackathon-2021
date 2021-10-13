import {
  ICardStyleProps,
  IBasicButtonStyles,
  CoherenceCardBasicButton,
  CardStandardHeader,
  Card,
} from "@cseo/controls";
import { CoherenceTheme } from "@cseo/styles";
import {
  IStyleFunctionOrObject,
  NeutralColors,
  FontSizes,
  FontWeights,
  IconButton,
  IIconProps,
  CommandBar,
  ICommandBarItemProps,
} from "@fluentui/react";
import React from "react";
import { useAppDispatch } from "../../app/hooks";
import CustomerMetrics from "./customerMetrics";
import {
  CustomerMetricInterface,
  MetricInterface,
  removeMetric,
} from "./metricsSlice";
import { getMetricColor } from "./metricUtils";

const CustomerMetricsCard = ({
  metrics,
}: {
  metrics: CustomerMetricInterface;
}): JSX.Element => {
  const dispatch = useAppDispatch();
  const buttonStyle: IStyleFunctionOrObject<
    ICardStyleProps,
    IBasicButtonStyles
  > = {
    root: [
      {
        width: "40px",
        height: "40px",
        padding: "0px",
        selectors: {
          ":hover": {
            background: CoherenceTheme.palette.neutralLighter,
            color: CoherenceTheme.palette.neutralPrimary,
            textDecoration: "none",
          },
          ":active": {
            background: CoherenceTheme.palette.neutralLight,
            color: CoherenceTheme.palette.neutralPrimary,
            textDecoration: "none",
          },
          ":active:hover": {
            background: CoherenceTheme.palette.neutralLight,
            color: CoherenceTheme.palette.neutralPrimary,
            textDecoration: "none",
          },
        },
      },
    ],
    icon: [
      {
        textAlign: "center",
        margin: "0px 4px",
        color: CoherenceTheme.palette.neutralPrimary,
      },
    ],
  };
  const counterStyle: IStyleFunctionOrObject<ICardStyleProps, {}> = {
    color: NeutralColors.gray200,
    alignSelf: "center",
    gridRow: "1",
    gridColumn: "2",
    overflow: "hidden",
    whiteSpace: "nowrap",
    paddingRight: "4px",
    fontSize: FontSizes.medium,
    fontWeight: FontWeights.semibold,
  };
  const CustomButton = (name: string) => {
    const deleteIcon: IIconProps = { iconName: "Delete" };
    return (
      <>
        {/* <CoherenceCardBasicButton
          linkProps={{
            "aria-label": "Delete button",
          }}
          iconName="Delete"
          styles={buttonStyle}
          onClick={() => dispatch(deleteMetric(name))}
        /> */}
        <IconButton
          iconProps={deleteIcon}
          title="Delete"
          ariaLabel="Delete"
          onClick={() => dispatch(removeMetric(name))}
        />
      </>
    );
  };

  const CustomCounter = () => {
    return <div style={counterStyle}>(28)</div>;
  };

  const getHeader = (title: string) => {
    return (
      <CardStandardHeader
        cardTitle={title}
        // contextProps={{
        //   customButton: CustomButton(title),
        // }}
        // subTitle={CustomCounter()}
      />
    );
  };
  const getFooter = () => {
    const _items: ICommandBarItemProps[] = [
      {
        key: "edit",
        text: "Edit",
        iconProps: { iconName: "Edit" },
        onClick: () => console.log("Edit"),
      },
    ];
    return <CommandBar items={_items} ariaLabel="Metric Actions" />;
  };
  return (
    <div
      style={{
        minWidth: "175px",
        minHeight: "150px",
        maxWidth: "400px",
        maxHeight: "100%",
      }}
    >
      <Card
        header={getHeader(metrics.customer)}
        //   footer={getFooter()}
      >
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            flexWrap: "wrap",
            justifyContent: "space-between",
            gap: "10px 10px",
          }}
        >
          {metrics.metrics.map((m: MetricInterface) => (
            <div>
              <span style={{ fontWeight: 700 }}>{m.name}: </span>
              <span style={{ color: getMetricColor(m) }}>{m.value}</span>
            </div>
          ))}
        </div>
      </Card>
    </div>
  );
};

export default CustomerMetricsCard;
