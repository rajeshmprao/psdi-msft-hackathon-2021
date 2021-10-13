import axios from "axios";
import { ServerResponse } from "node:http";
export enum httpMethods {
  get,
  post,
}
interface APIRequestInterface {
  uri: string;
  method: httpMethods;
  body?: Record<string, unknown>;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const apiAsync = async (request: APIRequestInterface): Promise<any> => {
  try {
    if (request.method === httpMethods.get) {
      const response = await axios.request<ServerResponse>({
        url: request.uri,
        headers: {
          __RequestVerificationToken: document.getElementsByName(
            "__RequestVerificationToken"
          )[0]?.nodeValue,
        },
      });
      return response.data;
    }
    if (request.method === httpMethods.post) {
      if (request.body) {
        const response = await axios.request<ServerResponse>({
          method: "POST",
          headers: {
            // __RequestVerificationToken: (
            //   document.getElementsByName(
            //     "__RequestVerificationToken"
            //   )[0] as HTMLInputElement
            // )?.value,
            "Content-Type": "application/json",
          },
          url: request.uri,
          data: request.body,
        });
        return response;
      }
    }
  } catch (e) {
    console.log("error");
  }
};
