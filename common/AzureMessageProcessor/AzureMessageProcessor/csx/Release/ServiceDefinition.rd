<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureMessageProcessor" generation="1" functional="0" release="0" Id="35ae9fdb-7dac-481d-b4a5-102efdc823ee" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureMessageProcessorGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/LB:MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapCertificate|MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:EmailQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:EmailQueueName" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.BlobStorage.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.BlobStorage.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:SendGridPassword" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:SendGridPassword" />
          </maps>
        </aCS>
        <aCS name="MessageProcessor:SendGridUserName" defaultValue="">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessor:SendGridUserName" />
          </maps>
        </aCS>
        <aCS name="MessageProcessorInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MapMessageProcessorInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapMessageProcessor:EmailQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/EmailQueueName" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.BlobStorage.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.BlobStorage.ConnectionString" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapMessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapMessageProcessor:SendGridPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/SendGridPassword" />
          </setting>
        </map>
        <map name="MapMessageProcessor:SendGridUserName" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/SendGridUserName" />
          </setting>
        </map>
        <map name="MapMessageProcessorInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessorInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="MessageProcessor" generation="1" functional="0" release="0" software="F:\NewSvn\AzureMessageProcessor\trunk\AzureMessageProcessor\csx\Release\roles\MessageProcessor" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/SW:MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="EmailQueueName" defaultValue="" />
              <aCS name="Microsoft.BlobStorage.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="SendGridPassword" defaultValue="" />
              <aCS name="SendGridUserName" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;MessageProcessor&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;MessageProcessor&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessorInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessorUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessorFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="MessageProcessorUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="MessageProcessorFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="MessageProcessorInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="6fd12c90-a808-43be-bc57-04ef67f3a81c" ref="Microsoft.RedDog.Contract\ServiceContract\AzureMessageProcessorContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="3de417a1-aef4-4e43-9dab-9a653d27e064" ref="Microsoft.RedDog.Contract\Interface\MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureMessageProcessor/AzureMessageProcessorGroup/MessageProcessor:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>