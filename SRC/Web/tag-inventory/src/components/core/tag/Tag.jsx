import React, { useState, useEffect } from "react";
import {
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Button
} from "@material-ui/core";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { getTag } from "../../../actions/tagActions";
import Actions from "../../common/reuseable/Actions";
import config from "../../../config.json";
import { Link } from "react-router-dom";
import TagDetail from "../tag/TagDetail";
import ShipmentDetail from "../shipment/ShipmentDetail";
import ReceivedBoxDetail from "../received-boxes/ReceivedBoxDetail";
import _ from "lodash";

const Tag = props => {
  const [shipmentStatusID, setShipmentStatusID] = useState(0);
  const tagID = props.match.params.tagID;

  useEffect(() => {
    const url = `${process.env.REACT_APP_API_URL}/tag/${tagID}`;
    props.getTagAction(url);
  }, []);

  const handleClick = arg => {
    switch (arg.target.action) {
      case "back":
        props.history.goBack();
        break;
    }
  };

  return (
    <React.Fragment>
      <div className="actions">
        <Actions onLinkClick="handleClick"></Actions>
      </div>
      <div className="title-summary-wrapper">
        <div className="content-title">
          <h3>{tagID}</h3>
        </div>
        {tagID !== -1 && (
          <div className="shipment-detail-summary">
            <span>Summary</span> &nbsp; | &nbsp;
            <Link to={`/tag/history/${tagID}`}>History</Link>
          </div>
        )}
      </div>
      <div className="content-area-tag-summary">
        <div className="row">
          <div className="col-4">
            <TagDetail tag={props.tag} readOnly={true}></TagDetail>
          </div>
          <div className="col-4">
            <ReceivedBoxDetail
              receivedBox={props.tag.receivedBox}
              readOnly={false}
            ></ReceivedBoxDetail>
          </div>
          <div className="col-4">
            <ShipmentDetail
              shipment={props.tag.receivedBox.shipment}
              shipmentID={props.tag.receivedBox.shipment.shipmentID}
              shipmentStatusID={props.tag.receivedBox.shipment.statusID}
              errors={{}}
              readOnly={true}
              onChangeValue={() => {}}
            ></ShipmentDetail>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

PropTypes.Tag = {
  tag: PropTypes.object.isRequired,
  getTag: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    tag: state.tag.tag
  };
};

const mapDispatchToProps = dispatch => ({
  getTagAction: url => dispatch(getTag(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Tag);
