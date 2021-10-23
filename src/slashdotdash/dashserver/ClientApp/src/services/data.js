import format from "date-fns/format";

const API_URL = "http://atomspeech.germanywestcentral.cloudapp.azure.com/dash";

export const fetchData = async () => {
  try {
    const res = await fetch(`${API_URL}/kpi`);
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

export const fetchShopsData = async (date) => {
  try {
    const res = await fetch(`${API_URL}/shop?planDate=${format(date, "yyyy-MM-dd")}`);
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

export const fetchShopData = async (date, id) => {
  console.log(date, id);
  try {
    const res = await fetch(`${API_URL}/shop/resourceGroup?planDate=${format(date, "yyyy-MM-dd")}&shopId=${id}`);
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
    const res = await fetch(`${API_URL}/plan?planDate=${format(date, "yyyy-MM-dd")}`);
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

export const fetchComparePlanData = async (dateSrc, dateDst) => {
  const query = `?srcPlanDate=${format(dateSrc, "yyyy-MM-dd")}&dstPlanDate=${format(dateDst, "yyyy-MM-dd")}`;
  try {
    const res = await fetch(`${API_URL}/plan/compare${query}`);
    if (res.status === 200) {
      return await res.json();
    }
    return null;
  } catch {
    return null;
  }
};

// /dash/shop/resourceGroup?planDate=2021-10-07&shopId=1
