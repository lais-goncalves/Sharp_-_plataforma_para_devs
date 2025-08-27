
import {useAuth} from "./AuthContext";

export const ProfileFetcher = () => {
  const { fetchProfile } = useAuth();
  return <div>Profile fetcher</div>;
};