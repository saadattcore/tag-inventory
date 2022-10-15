import React, { useState, useEffect } from "react";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import Snackbar from "@material-ui/core/Snackbar";
import withAlert from "./withAlert";

const AppAlert = ({
  type,
  message,
  title,
  duration,
  displayAlert,
  onAlertClose
}) => {
  //const [show, setShow] = useState(displayAlert);

  /* useEffect(() => {
    alert("UseEffect");
    setShow(displayAlert);
  }, [displayAlert]); */

  //const onAlertClose = () => setShow(false);

  return (
    <React.Fragment>
      <Snackbar
        open={displayAlert}
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        autoHideDuration={duration}
        onClose={e => onAlertClose({ initState: false })}
      >
        <Alert
          severity={type}
          onClose={e => onAlertClose({ initState: false })}
        >
          <AlertTitle>{title}</AlertTitle>
          <strong>{message}</strong>
        </Alert>
      </Snackbar>
    </React.Fragment>
  );
};

export default AppAlert;
