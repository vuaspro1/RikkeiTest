// export const API_URL = "https://api-qltv-dev.bizlib.vn";
export const API_URL = "https://localhost:82";

export const ERROR_MESSEGE_NULL = " không được trống!";
export const ERROR_MESSEGE_INVALID = " không đúng định dạng!";
export const ERROR_MESSEGE_POSITIVE_NUMBERS = " không được âm!";
export const LENGTH_MAX = 225;
export const LENGTH_MIN = 4;
export const LIMIT_TIME = 20;

// eslint-disable-next-line import/no-anonymous-default-export
export default {
	process: {
		env: {
			API_URL,
			ERROR_MESSEGE_NULL,
			LENGTH_MAX,
			LENGTH_MIN,
			LIMIT_TIME,
		},
	},
};
