import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { apiAsync, httpMethods } from "../../app/utils";

export interface MetricInterface {
  value?: string;
  name: string;
  upperThreshold: string;
  lowerThreshold: string;
  subscribe: string;
}

export interface CustomerMetricInterface {
  customer: string;
  metrics: MetricInterface[];
}

export interface MetricCustomizationPanelInterface {
  isOpen: boolean;
  metric?: string;
}
interface MetricsSliceInterface {
  portfolioMetrics: MetricInterface[];
  allMetrics: string[];
  metricCustomizationPanel: MetricCustomizationPanelInterface;
  customerMetrics: CustomerMetricInterface[];
}
const initialState: MetricsSliceInterface = {
  portfolioMetrics: [],
  allMetrics: [],
  metricCustomizationPanel: {
    isOpen: false,
    metric: undefined,
  },
  customerMetrics: [],
};

export const getPortfolioMetrics = createAsyncThunk(
  "metrics/portfolioMetrics",
  async () => {
    const response: MetricInterface[] = await apiAsync({
      uri: "/api/metrics/portfolioMetrics",
      method: httpMethods.get,
    });
    return response;
  }
);

export const getCustomerMetrics = createAsyncThunk(
  "metrics/customerMetrics",
  async () => {
    const response: CustomerMetricInterface[] = await apiAsync({
      uri: "/api/metrics/customerMetrics",
      method: httpMethods.get,
    });
    return response;
  }
);

export const getAllMetrics = createAsyncThunk(
  "metrics/allMetrics",
  async () => {
    const response: string[] = await apiAsync({
      uri: "/api/metrics/allMetrics",
      method: httpMethods.get,
    });
    return response;
  }
);

export const addMetric = createAsyncThunk(
  "metrics/addMetric",
  async (body: MetricInterface, thunkAPI) => {
    await apiAsync({
      uri: "/api/metrics/addMetric",
      method: httpMethods.post,
      body: body as unknown as Record<string, unknown>,
    });
    thunkAPI.dispatch(updateMetricCustomizationPanel({ isOpen: false }));
    thunkAPI.dispatch(getPortfolioMetrics());
    thunkAPI.dispatch(getCustomerMetrics());
  }
);

export const editMetric = createAsyncThunk(
  "metrics/editMetric",
  async (body: MetricInterface, thunkAPI) => {
    await apiAsync({
      uri: "/api/metrics/editMetric",
      method: httpMethods.post,
      body: body as unknown as Record<string, unknown>,
    });
    thunkAPI.dispatch(updateMetricCustomizationPanel({ isOpen: false }));
    thunkAPI.dispatch(getPortfolioMetrics());
    thunkAPI.dispatch(getCustomerMetrics());
  }
);

export const subscribeMetric = createAsyncThunk(
  "metrics/subscribeMetric",
  async (body: MetricInterface, thunkAPI) => {
    await apiAsync({
      uri: "/api/metrics/subscribeMetric",
      method: httpMethods.post,
      body: body as unknown as Record<string, unknown>,
    });
    thunkAPI.dispatch(getPortfolioMetrics());
    thunkAPI.dispatch(getCustomerMetrics());
  }
);

export const removeMetric = createAsyncThunk(
  "metrics/removeMetric",
  async (name: string, thunkAPI) => {
    await apiAsync({
      uri: `/api/metrics/removeMetric?metric=${name}`,
      method: httpMethods.get,
    });

    thunkAPI.dispatch(getPortfolioMetrics());
    thunkAPI.dispatch(getCustomerMetrics());
  }
);

export const metricsSlice = createSlice({
  name: "metrics",
  initialState,
  reducers: {
    updateMetricCustomizationPanel: (
      state,
      action: PayloadAction<MetricCustomizationPanelInterface>
    ) => {
      state.metricCustomizationPanel = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(getPortfolioMetrics.fulfilled, (state, action) => {
      state.portfolioMetrics = action.payload;
    });
    builder.addCase(getAllMetrics.fulfilled, (state, action) => {
      state.allMetrics = action.payload;
    });
    builder.addCase(getCustomerMetrics.fulfilled, (state, action) => {
      state.customerMetrics = action.payload;
    });
  },
});

export const selectPortfolioMetrics = (state: RootState): MetricInterface[] =>
  state.metrics.portfolioMetrics;
export const selectUserUnselectedMetrics = (state: RootState): string[] =>
  state.metrics.allMetrics.filter(
    (m) => !state.metrics.portfolioMetrics.map((pm) => pm.name).includes(m)
  );
export const selectMetricCustomizationPanelDetails = (
  state: RootState
): MetricCustomizationPanelInterface => state.metrics.metricCustomizationPanel;

export const isNewMetricCustomization = (state: RootState): boolean =>
  state.metrics.metricCustomizationPanel.metric === null;
export const selectCustomerMetrics = (
  state: RootState
): CustomerMetricInterface[] => state.metrics.customerMetrics;

export const { updateMetricCustomizationPanel } = metricsSlice.actions;

export default metricsSlice.reducer;
