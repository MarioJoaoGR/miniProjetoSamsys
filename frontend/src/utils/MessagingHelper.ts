import { toast } from "react-toastify";

export interface MessagingHelper<T = null> {
  success: boolean;
  message: string;
  errorType?: string;
  obj?: T;
}

export class MessagingHelperHandler {
  static handleResponse<T>(response: MessagingHelper<T>) {
    if (response.success) {
      toast.success(response.message, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "colored",
      });
    } else {
      toast.error(response.message, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "colored",
      });
    }
  }
}
