import format from "date-fns/format";

export const fetchData = async () => {
  try {
    const res = await fetch("http://atomspeech.germanywestcentral.cloudapp.azure.com/dash/kpi");
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

export const fetchShopData = async (date) => {
  try {
    const res = await fetch(
      `http://atomspeech.germanywestcentral.cloudapp.azure.com/dash/shop?planDate=${format(date, "yyyy-MM-dd")}`,
    );
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

export const fetchPlanData = async (date) => {
  try {
    const res = await fetch(
      `http://atomspeech.germanywestcentral.cloudapp.azure.com/dash/plan?planDate=${format(date, "yyyy-MM-dd")}`,
    );
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};
