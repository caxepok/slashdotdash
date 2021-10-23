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
